using System.ComponentModel.DataAnnotations;

namespace UploadExcel.Models
{
    public class Property
    {
        [Key] // Add this attribute to designate the primary key
        public int Id { get; set; } // Or choose any other unique property to be the key
        public string ? Property_contract_number { get; set; }
        public string ?Bp_number { get; set; }
        public string ?Property_type { get; set; }
        public string ?Project { get; set; }
        public string ?Building_Phase { get; set; }
        public string ?Floor_Block { get; set; }
        public string ?Unit_Code { get; set; }
        public string ?View { get; set; }
        public string ?Unit_Type { get; set; }
        public decimal? Unit_Area { get; set; }
        public decimal? Balcony_Area { get; set; }
        public decimal? Total_Unit_Area { get; set; }
        public string ?Status_In_General { get; set; }
        public string ?Milestone { get; set; }
        public string ?Status_Color { get; set; }
    }
}
