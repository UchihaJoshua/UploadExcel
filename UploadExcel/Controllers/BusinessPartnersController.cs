using Microsoft.AspNetCore.Mvc;
using UploadExcel.Data;

namespace UploadExcel.Controllers
{
    public class BusinessPartnersController : Controller
    {
        private readonly ApplicationDbContext context;

        public BusinessPartnersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var bpPartners = context.business_partners
            .Where(bpPartners => bpPartners.role == "Principal Buyer")
            .ToList();

            return View(bpPartners);
        }

    }
}
