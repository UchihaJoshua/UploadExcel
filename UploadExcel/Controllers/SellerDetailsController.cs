using Microsoft.AspNetCore.Mvc;
using UploadExcel.Data;
using UploadExcel.Models;
using System.Linq;

namespace UploadExcel.Data.Controllers
{
    public class SellerDetailsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public SellerDetailsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult View(string SearchBy, string SearchTerm)
        {
            ViewData["ControllerName"] = "SellerDetailsController";

            var sellers = from s in context.sellers
                          select s;

            if (!string.IsNullOrWhiteSpace(SearchBy) && !string.IsNullOrWhiteSpace(SearchTerm))
            {
                string normalizedSearchTerm = SearchTerm.Trim().ToLower();

                if (SearchBy == "bp_number")
                {
                    sellers = sellers.Where(s =>
                        !string.IsNullOrWhiteSpace(s.seller_buyer_reference) &&
                        s.seller_buyer_reference.Trim().ToLower() == normalizedSearchTerm);
                }
                else if (SearchBy == "sales_contract_number")
                {
                    sellers = sellers.Where(s =>
                        !string.IsNullOrWhiteSpace(s.sales_contract_number) &&
                        s.sales_contract_number.Trim().ToLower() == normalizedSearchTerm);
                }
            }

            var orderedSellers = sellers.OrderByDescending(s => s.Id).ToList();
            return View(orderedSellers);
        }
    }
}
