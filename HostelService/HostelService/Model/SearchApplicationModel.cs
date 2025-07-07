namespace Model
{
    public class SearchApplicationModel : FilterParamModel
    {        
        public int? AcademicYear { get; set; }      
        public int? CourseType { get; set; }       
        public int? CasteCategory { get; set; }       
        public int? Caste { get; set; }       
        public int? Status { get; set; }       
        public int? CourseYear { get; set; }      
        public string? ApplicationNo { get; set; }
        public string? Name { get; set; }      
        public int? CasteId { get; set; }
        public int? ServiceType { get; set; }
        public int? IsNewApplicant { get; set; }

        public int? InstallmentNo { get; set; }
        public int? AYId {  get; set; }
    }

    public class SearchAllotmentStatusModel : FilterParamModel
    {
        public int? HostelId { get; set; }
        public int? CourseType { get; set; }
        public int? CasteCategory { get; set; }
        public int? Status { get; set; }
        public int? CasteId { get; set; }        
        public int? QuotaId { get; set; }        
    }

    public class SearchApplicationOfflineModel : FilterParamModel
    {
        public int? AcademicYear { get; set; }
        public string? ApplicationNo { get; set; }
        public string? Name { get; set; }       
    }

    public class WardenApplicationModel
    {
        public string UserId { get; set; }
        public string ApplicationNo { get; set; } = "";
        public string ApplicantName { get; set; } = "";
        public string Category { get; set; } = "";
        public string CurrentCourse { get; set; } = "";
        public string CurrentCourseYear { get; set; } = "";
        public string Nameofcollege { get; set; } = "";
        public string Addressofcollege { get; set; } = "";
        public string HostelpreferenceNo1 { get; set; } = "";
        public string OrphanDisabled { get; set; } = "";
        public string HostelName { get; set; } = "";
        public string SSCPercentageMarks { get; set; } = "";
        public string HSCPercentageMarks { get; set; } = "";
        public string UnderGraduatePercentageMarks { get; set; } = "";
        public string PostGraduatePercentageMarks { get; set; } = "";
        public string PreStandardPrecentage { get; set; } = "";
    }
}
