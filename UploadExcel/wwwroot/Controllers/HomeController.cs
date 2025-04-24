using Microsoft.Data.SqlClient;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Collections.Generic;
using System.Data;
using UploadExcel.Models;
using UploadExcel.Data;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace UploadExcel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString = "Data Source=.;Initial Catalog=InventoryDB;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True";

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> UploadExcel(string selectedTable)
        {
            var tableNames = await GetTableNamesFromDatabase();
            ViewBag.TableNames = tableNames;

            if (!string.IsNullOrEmpty(selectedTable))
            {
                var columns = await GetColumnsFromTable(selectedTable);
                ViewBag.TableColumns = columns;
            }
            else
            {
                ViewBag.TableColumns = null;
            }

            ViewBag.SelectedTable = selectedTable;
            return View();
        }

        private async Task<List<string>> GetTableNamesFromDatabase()
        {
            var tableNames = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME != 'sysdiagrams'", connection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    tableNames.Add(reader.GetString(0));
                }
            }

            return tableNames;
        }

        private async Task<List<(string ColumnName, string DataType, bool IsIdentity)>> GetColumnsFromTable(string tableName)
        {
            var columns = new List<(string ColumnName, string DataType, bool IsIdentity)>();
            string fullyQualifiedName = $"dbo.{tableName}";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = $@"
                    SELECT COLUMN_NAME, DATA_TYPE,
                           CASE 
                               WHEN COLUMNPROPERTY(OBJECT_ID('{fullyQualifiedName}'), COLUMN_NAME, 'IsIdentity') = 1 THEN 1
                               ELSE 0
                           END AS IsIdentity
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @tableName";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tableName", tableName);

                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        columns.Add((
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetInt32(2) == 1
                        ));
                    }
                }
            }

            return columns;
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcel(string selectedTable, IFormFile file)
        {
            const string ERROR = "error";
            const string SUCCESS = "success";

            if (string.IsNullOrEmpty(selectedTable))
            {
                ViewBag.Message = "⚠️ Please select a destination table from the dropdown.";
                ViewBag.MessageType = ERROR;
                ViewBag.TableNames = await GetTableNamesFromDatabase();
                return View();
            }

            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "⚠️ Please upload a non-empty Excel file.";
                ViewBag.MessageType = ERROR;
                ViewBag.TableNames = await GetTableNamesFromDatabase();
                return View();
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var filePath = Path.Combine(uploadFolder, file.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var columns = await GetColumnsFromTable(selectedTable);

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    bool isHeaderSkipped = false;

                    while (reader.Read())
                    {
                        if (!isHeaderSkipped)
                        {
                            isHeaderSkipped = true;
                            continue;
                        }

                        var nonIdentityColumns = columns
                            .Where(c => !IsIdentityColumn(selectedTable, c.ColumnName))
                            .ToList();

                        bool hasCreatedAt = nonIdentityColumns.Any(c => c.ColumnName.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase));

                        var excelColumns = nonIdentityColumns
                            .Where(c => !c.ColumnName.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        var columnNames = string.Join(",", excelColumns.Select(c => $"[{c.ColumnName}]"));
                        var parameterNames = string.Join(",", excelColumns.Select(c => "@" + c.ColumnName));

                        if (hasCreatedAt)
                        {
                            columnNames += ", [CreatedAt]";
                            parameterNames += ", @CreatedAt";
                        }

                        var commandText = $"INSERT INTO [{selectedTable}] ({columnNames}) VALUES ({parameterNames})";

                        using (var connection = new SqlConnection(_connectionString))
                        {
                            await connection.OpenAsync();
                            using (var command = new SqlCommand(commandText, connection))
                            {
                                foreach (var column in excelColumns)
                                {
                                    int columnIndex = columns.IndexOf(column);
                                    var value = reader.IsDBNull(columnIndex) ? DBNull.Value : reader.GetValue(columnIndex);
                                    var parameterName = $"@{column.ColumnName}";

                                    value = TryParseValue(value, column.DataType);
                                    command.Parameters.AddWithValue(parameterName, value ?? DBNull.Value);
                                }

                                if (hasCreatedAt)
                                {
                                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                                }

                                await command.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }

                ViewBag.Message = "✅ Data uploaded successfully!";
                ViewBag.MessageType = SUCCESS;
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"❌ An error occurred while processing the file: {ex.Message}";
                ViewBag.MessageType = ERROR;
            }

            ViewBag.TableNames = await GetTableNamesFromDatabase();
            return View();
        }

        private object TryParseValue(object value, string dataType)
        {
            if (value == null || value == DBNull.Value)
                return DBNull.Value;

            try
            {
                switch (dataType.ToLower())
                {
                    case "int":
                    case "bigint":
                    case "smallint":
                        return Convert.ToInt32(value);

                    case "tinyint":
                        return Convert.ToByte(value);

                    case "bit":
                        if (value is string str)
                        {
                            if (bool.TryParse(str, out var boolResult))
                                return boolResult;
                            if (int.TryParse(str, out var intResult))
                                return intResult == 1;
                        }
                        return Convert.ToBoolean(value);

                    case "decimal":
                    case "numeric":
                    case "money":
                    case "smallmoney":
                        return Convert.ToDecimal(value);

                    case "float":
                        return Convert.ToDouble(value);

                    case "real":
                        return Convert.ToSingle(value);

                    case "date":
                    case "datetime":
                    case "datetime2":
                    case "smalldatetime":
                        return Convert.ToDateTime(value);

                    case "char":
                    case "nchar":
                    case "varchar":
                    case "nvarchar":
                    case "text":
                    case "ntext":
                        return value.ToString();

                    case "uniqueidentifier":
                        return Guid.Parse(value.ToString());

                    default:
                        return value;
                }
            }
            catch
            {
                return DBNull.Value;
            }
        }

        private bool IsIdentityColumn(string tableName, string columnName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string fullyQualifiedName = $"dbo.{tableName}";

                var query = $"SELECT COLUMNPROPERTY(OBJECT_ID('{fullyQualifiedName}'), @columnName, 'IsIdentity')";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@columnName", columnName);
                    var result = command.ExecuteScalar();
                    return result != DBNull.Value && Convert.ToInt32(result) == 1;
                }
            }
        }
    }
}
