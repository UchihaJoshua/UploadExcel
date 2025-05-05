using Microsoft.AspNetCore.Mvc;
using UploadExcel.Data;
using UploadExcel.Models;
using System.Linq;

namespace UploadExcel.wwwroot.Controllers
{
    [Route("[controller]")]
    public class PropertyDetailsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public PropertyDetailsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        [HttpGet("PropertyView")]
        public IActionResult PropertyView(string SearchBy, string SearchTerm)
        {
            ViewData["ControllerName"] = "PropertyDetailsController";

            // Fetch properties
            var properties = from p in context.Properties
                             select p;

            // Apply search filters
            if (!string.IsNullOrWhiteSpace(SearchBy) && !string.IsNullOrWhiteSpace(SearchTerm))
            {
                string normalizedSearchTerm = SearchTerm.Trim().ToLower();

                if (SearchBy == "bp_number")
                {
                    properties = properties.Where(p =>
                        !string.IsNullOrWhiteSpace(p.Bp_number) &&
                        p.Bp_number.Trim().ToLower() == normalizedSearchTerm);
                }
                else if (SearchBy == "sales_contract_number")
                {
                    properties = properties.Where(p =>
                        !string.IsNullOrWhiteSpace(p.Property_contract_number) &&
                        p.Property_contract_number.Trim().ToLower() == normalizedSearchTerm);
                }
            }

            // Execute property query first (into memory)
            var orderedProperties = properties.OrderByDescending(p => p.Id).ToList();

            // Now fetch credit reviews from DB (could be filtered further if needed)
            var allCreditReviews = context.Credit_Review.ToList();

            // Perform the join in memory using LINQ to Objects
            var creditReviews = from cr in allCreditReviews
                                join p in orderedProperties
                                on cr.sales_contract_number?.Trim().ToLower()
                                equals p.Property_contract_number?.Trim().ToLower()
                                select cr;

            // Log count
            Console.WriteLine($"Found {creditReviews.Count()} credit reviews matching property contract number.");

            // No result condition
            if (!orderedProperties.Any() || !creditReviews.Any())
            {
                return View("NoResults");
            }

            // Populate view model
            var viewModel = new PropertyDetailsViewModel
            {
                Properties = orderedProperties,
                CreditReviews = creditReviews.ToList()
            };

            return View(viewModel);
        }


    }
}
