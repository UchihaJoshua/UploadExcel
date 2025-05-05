using Microsoft.AspNetCore.Mvc;
using UploadExcel.Data;
using UploadExcel.Models;
using System.Linq;

namespace UploadExcel.Controllers // ✅ Correct namespace
{
    [Route("[controller]")]
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext context;

        public DocumentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("DocumentView")]
        public IActionResult DocumentView(string SearchBy, string SearchTerm)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                return BadRequest("Search term is required.");
            }

            var query = context.document_submitted.AsQueryable();

            if (SearchBy?.ToLower() == "bp_number")
            {
                query = query.Where(d => d.bp_number != null && d.bp_number.Trim().ToLower() == SearchTerm.Trim().ToLower());
            }
            else if (SearchBy?.ToLower() == "sales_contract_number")
            {
                query = query.Where(d => d.sales_contract_number != null && d.sales_contract_number.Trim().ToLower() == SearchTerm.Trim().ToLower());
            }
            else
            {
                return BadRequest("Invalid search field.");
            }

            var documents = query.ToList();

            if (!documents.Any())
            {
                return NotFound("No matching documents found.");
            }

            return View(documents); // ✅ Render Razor view
        }
    }
}
