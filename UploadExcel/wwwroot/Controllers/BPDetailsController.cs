using Microsoft.AspNetCore.Mvc;
using UploadExcel.Data;
using UploadExcel.Models;
using System.Linq;

namespace UploadExcel.Controllers
{
    public class BPDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BPDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Updated View method with SearchBy
        public IActionResult View(string SearchTerm, string SearchBy)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm) || string.IsNullOrWhiteSpace(SearchBy))
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest("Please enter a valid value and choose a search option.");
                }

                ViewData["Error"] = "Please enter a valid value and choose a search option.";
                return View();
            }

            BussinessPartners principal = null;

            // Search based on selected criteria
            if (SearchBy == "bp_number")
            {
                principal = _context.business_partners
                    .FirstOrDefault(bpPartners => bpPartners.bp_number == SearchTerm && bpPartners.role == "Principal Buyer");
            }
            else if (SearchBy == "sales_contract_number")
            {
                principal = _context.business_partners
                    .FirstOrDefault(bpPartners => bpPartners.sales_contract_number == SearchTerm && bpPartners.role == "Principal Buyer");
            }

            if (principal == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return NotFound($"No Principal Buyer found for '{SearchTerm}'.");
                }

                ViewData["Error"] = $"No Principal Buyer found for '{SearchTerm}'.";
                return View();
            }

            var coBuyers = _context.business_partners
                .Where(bpPartners => bpPartners.role == "Co-Buyer" && bpPartners.principal_buyer_reference == principal.bp_number)
                .ToList();

            var spouses = _context.business_partners
                .Where(bpPartners => bpPartners.role.Contains("Spouse") && bpPartners.principal_buyer_reference == principal.bp_number)
                .ToList();

            var viewModel = new BuyerDetailsViewModel
            {
                PrincipalBuyer = principal,
                CoBuyers = coBuyers,
                Spouses = spouses
            };

            ViewData["SearchResultCount"] = 1 + coBuyers.Count + spouses.Count;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("View", viewModel);
            }

            return View(viewModel);
        }
    }
}
