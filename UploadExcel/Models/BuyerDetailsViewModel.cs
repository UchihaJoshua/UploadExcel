namespace UploadExcel.Models
{
    public class BuyerDetailsViewModel
    {
        public BussinessPartners PrincipalBuyer { get; set; }
        public List<BussinessPartners> CoBuyers { get; set; }
        public List<BussinessPartners> Spouses { get; set; }
        public Seller Seller { get; set; }
    }
}
