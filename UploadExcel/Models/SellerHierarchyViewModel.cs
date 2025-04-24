namespace UploadExcel.Models
{
    public class SellerHierarchyViewModel
    {
        public Seller LowestLevelSeller { get; set; }
        public List<Seller> DirectReports { get; set; }
    }
}
