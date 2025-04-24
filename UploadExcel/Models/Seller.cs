using System;
using System.ComponentModel.DataAnnotations;

namespace UploadExcel.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; } // Use your real PK or create a surrogate key

        public string? fullname { get; set; }
        public string? seller_type_roles { get; set; }
        public int seller_level { get; set; }

        public string? seller_buyer_reference { get; set; }
        public int? reporting_to { get; set; }
        public string? sales_contract_number { get; set; }
    }
}
