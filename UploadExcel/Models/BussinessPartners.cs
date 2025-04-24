namespace UploadExcel.Models
{
    public class BussinessPartners
    {

        public string? sales_contract_number { get; set; }  // Nullable string
        public string? role { get; set; }  // Nullable string
        public string? bp_number { get; set; }  // Nullable string
        public string? fullname { get; set; }  // Nullable string
        public string? principal_buyer_reference { get; set; }  // Nullable string
        public DateOnly? birthdate { get; set; }  // Nullable DateOnly
        public int? age { get; set; }  // Nullable int
        public string? age_category { get; set; }  // Nullable string
        public string? gender { get; set; }  // Nullable string
        public string? civil_status { get; set; }  // Nullable string
        public string? Email { get; set; }  // Nullable string
        public string? contact_number { get; set; }  // Nullable string
        public string? client_base { get; set; }  // Nullable string
        public string? citizenship { get; set; }  // Nullable string
        public string? nationality { get; set; }  // Nullable string
        public string? job_title { get; set; }  // Nullable string
        public string? demographic_by_market { get; set; }  // Nullable string
        public string? employment_category { get; set; }  // Nullable string
        public string? company_name { get; set; }  // Nullable string
        public string? industry_type { get; set; }  // Nullable string
        public string? employment_country { get; set; }  // Nullable string
        public string? reason_for_purchase { get; set; }  // Nullable string
        public int? number_of_homes_in_ph { get; set; }  // Nullable int
        public string? with_other_cpgi_units { get; set; }  // Nullable string
        public string? cpgi_unit_no { get; set; }  // Nullable string
        public string? with_dependents { get; set; }  // Nullable string
        public string? no_of_dependents { get; set; }  // Nullable string
        public decimal? income_declared_cb { get; set; }  // Nullable decimal
        public decimal? income_declared_pb { get; set; }  // Nullable decimal
        public decimal? total_income { get; set; }  // Nullable decimal
    }
}
