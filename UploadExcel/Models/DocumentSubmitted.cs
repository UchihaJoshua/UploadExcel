using System;

namespace UploadExcel.Models
{
    public class DocumentSubmitted
    {

        public string ? sales_contract_number { get; set; }

        public string ? bp_number { get; set; }

        public string ? unit_code { get; set; }

        public string ? document_code { get; set; }

        public DateTime? document_date_submitted { get; set; }

        public string ? yes_or_no { get; set; }
    }
}
