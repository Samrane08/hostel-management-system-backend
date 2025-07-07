namespace Model
{
    public class AllotWaitApplicationModel
    {
        public string ApplicationNo { get; set; } = "";
        public string ApplicantFullName { get; set; } = "";
        public string Percentage { get; set; } = "";
        public string ClassAdmission { get; set; } = "";
        public string Status { get; set; } = "";
        public string Caste { get; set; } = "";
    }

    public class AllotmentModel
    {
        public string ApplicationNo { get; set; } = "";
        public string Name { get; set; } = "";
        public string CourseType { get; set; } = "";
        public string Category { get; set; } = "";
        public string Caste { get; set; } = "";
        public string Quota { get; set; } = "";
        public string AdmissionStatus { get; set; } = "";
    }
    public class AllotmentCountModel
    {
        public int Id { get; set; }
        public int? CastId { get; set; }
        public string? Name { get; set; }
        public int? Percentage { get; set; }
        public int? Seat { get; set; }
        public int? Quota { get; set; }
        public int? Count { get; set; }
    }
    public class CastwiseCountModel: AllotmentCountModel
    {
        public int? Priority { get; set; }
    }
}
