namespace UploadExcel.Models
{
    public class CreditReview
    {
        public string? sales_contract_number { get; set; }
        public string? bp_number { get; set; }
        public string? project { get; set; }
        public string ?building_phase { get; set; }
        public string ?floor_block { get; set; }
        public string ?unit_code { get; set; }
        public string ?ci_result { get; set; }
        public string ?cmap_result { get; set; }
        public string ?ci_remarks { get; set; }
        public string ?ndi_status { get; set; }
        public string ?is_bank_approvable { get; set; }
        public string ?red_tag { get; set; }
        public string ?red_tag_reason { get; set; }
        public DateTime? ci_completion_date { get; set; }
        public string ?type_of_income { get; set; }
        public string? ndi_rate { get; set; }
        public decimal? net_disposable_income { get; set; }
        public decimal? other_loans { get; set; }
        public decimal? net_ndi { get; set; }
        public decimal? ndi_vs_bank_ma_tob_amt { get; set; }
        public string? percent_of_ndi_vs_ma { get; set; }
        public string ?ndi_category { get; set; }
        public int? max_term { get; set; }
        public string? extimated_max_term { get; set; }
    }
}
