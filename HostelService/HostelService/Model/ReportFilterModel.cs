namespace Model
{
    public class ReportFilterModel: FilterParamModel
    {
        public int? AcademicYear { get; set; }
        public int? HostelId { get; set; }
        public int? Caste { get; set; }
        public int? CasteCategory { get; set; }
        public int? DistrictId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }       
        public bool? Print { get; set; }       
    }
}
