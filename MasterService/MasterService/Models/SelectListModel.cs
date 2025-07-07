namespace MasterService.Models
{
    public class SelectListModel
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
        public string? FinancialYear { get; set; }
    }


    public class SelectDashboardManualModel
    {
        public string? id { get; set; }
        public string? Overview { get; set; }
        public string? Eligibility { get; set; }
        public string? Documents_Required { get; set; }
    }
    public class SelectList
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
       
    }

    public class MPSCBasicDetails
    {
        public List<SelectList> CasteCategory { get; set; }
        public List<SelectList> coaching { get; set; }
        public List<SelectList> gender { get; set; }
        public List<SelectList> interviewsponsorship { get; set; }
        public List<SelectList> mainsponsorship { get; set; }
        public List<SelectList> options { get; set; }
        public List<SelectList> sponsorship { get; set; }
        public List<SelectList> sponsorship_10000 { get; set; }

    }

    public class AcademicYearSwitch
    {
        public string ? ConsiderAcademicYear { get; set; }
    }

}
