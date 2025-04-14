using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UploadExcel.Migrations
{
    /// <inheritdoc />
    public partial class BpMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BussinessPartners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bp_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    principal_buyer_reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    age_category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    civil_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    citizenship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    job_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    demographic_by_market = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employment_category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    industry_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employment_country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reason_for_purchase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    number_of_homes_in_ph = table.Column<int>(type: "int", nullable: false),
                    with_other_cpgi_units = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cpgi_unit_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    with_dependents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    no_of_dependents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    income_declared_cb = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    income_declared_pb = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    total_income = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BussinessPartners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BussinessPartners");
        }
    }
}
