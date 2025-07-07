namespace Model.LotteryModel
{
    public class MeritApproveModel : FilterParamModel
    {
        public int? HostelId { get; set; }
        public int? CourseType { get; set; }
    }

    public class MeritRejectModel : FilterParamModel
    {
        public int? CourseType { get; set; }
    }

    public class MeritResponseModel
    {
        public int? ApplicationId { get; set; }
        public string? ApplicantName { get; set; }
        public string? Percentage { get; set; }
        public string? Category { get; set; }
        public string? Caste { get; set; }
        public string? HostelName { get; set; }
        public string? ApplicationNo { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public string? Districtname { get; set; }
        public string? serviceType { get; set; }
        public string? ApplicationType { get; set; }
        public string? Course { get; set; }
    }
}
