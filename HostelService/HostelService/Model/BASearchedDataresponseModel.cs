namespace Model
{
    public class BASearchedDataresponseModel1
    {
        public string ApplicationNo { get; set; }
        public string ApplicantFullName { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string HostelName { get; set; }
        public string CourseType { get; set; }
        public string ApplicationType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CasteCategoryName { get; set; }
        public string CasteName { get; set; }
        public string ScrutinyStatus { get; set; }
        public int? WorkFlowId { get; set; }
        

    }

    public class BASearchedDataresponseModel2
    {
        public string ApplicationNo { get; set; }
        public string ApplicantFullName { get; set; }
        public string DivisionName { get; set; }
        public string DistrictName { get; set; }
        public string CourseType { get; set; }
        public string ApplicationType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CasteCategoryName { get; set; }
        public string CasteName { get; set; }
        public string ScrutinyStatus { get; set; }
        public int? WorkFlowId { get; set; }

    }


    public class BASearchedDataresponseModel
    {
        public class BASearchedDataresponseModel1
        {
            public string ApplicationNo { get; set; }
            public string ApplicantFullName { get; set; }
            public string DivisionName { get; set; }
            public string DistrictName { get; set; }
            public string HostelName { get; set; }
            public string CourseType { get; set; }
            public string ApplicationType { get; set; }
            public DateTime? CreatedOn { get; set; }
            public string CasteCategoryName { get; set; }
            public string CasteName { get; set; }
            public string ScrutinyStatus { get; set; }
            public int? WorkFlowId { get; set; }
            public List<BASearchedDataresponseModel2>? lst { get; set;}

        }

        
    }

    
}
