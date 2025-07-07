namespace Model
{
    public class SearchApplicationReponseModel
    {
        public List<ApplicationModel>? List { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }

    public class ApplicationModel
    {
        public long Id { get; set; }
        public string ApplicationNo { get; set; } = "";
        public string AppAllotmentId { get; set; } = "";
        public string? HostelName { get; set; } = "";
        public string? ApplicationType { get; set; } = "";
        public string Name { get; set; } = "";
        public string CasteCategory { get; set; } = "";
        public string Caste { get; set; } = "";
        public string? ServiceName { get; set; } = "";
        public string? DistrictName { get; set; } = "";
        public string ApplicationDate { get; set; } = "";
        public string Status { get; set; } = "";
        public string ApplicationStatus { get; set; } = "";
        public int? IsAdmittedRejected { get; set; }
        public string ScrutinyStatus { get; set; }="" ;

        public int ServiceType { get; set; }

        public bool IsNewApplicant { get; set; }
    }    
}