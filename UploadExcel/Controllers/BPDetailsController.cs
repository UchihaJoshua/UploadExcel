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
            this._context = context;
        }

        public IActionResult View(string Pb_number)
        {
            if (string.IsNullOrWhiteSpace(Pb_number))
            {
                return BadRequest("Invalid Principal Buyer number.");
            }

            // Get the principal buyer
            var principal = _context.business_partners
                .FirstOrDefault(bpPartners => bpPartners.bp_number == Pb_number && bpPartners.role == "Principal Buyer");

            if (principal == null)
            {
                return NotFound($"Principal Buyer with bp_number '{Pb_number}' not found.");
            }

            // ✅ Correct this line — match reference to principal.bp_number
            var coBuyer = _context.business_partners
                .FirstOrDefault(bpPartners =>
                    bpPartners.role == "Co-Buyer/Spouse" &&
                    bpPartners.principal_buyer_reference == principal.bp_number);


            var viewModel = new BuyerDetailsViewModel
            {
                PrincipalBuyer = principal,
                CoBuyer = coBuyer
            };

            return View(viewModel);
        }

    }
}
