namespace Model
{
    public class ScrutinyModel
    {
        public long ApplicationId { get; set; }
        public int ActionId { get; set; }
        public string? Remark { get; set; }
        public List<int>? RejectionId { get; set; }
    }

    public class SrutinyResultModel
    {
        public long? Id { get; set; }
        public string? Message { get; set; }
    }
    public class SrutinyRemarkModel
    {
        public long? Id { get; set; }
        public string? HostelName { get; set; }
        public string? Prefrence { get; set; }
        public string? ActionName { get; set; }
        public string? ActionTaken { get; set; }
        public int? ScrutinyStatus { get; set; }
        public string? Remark { get; set; }
        public DateTime? ActionDate { get; set; }
        public string ActionDateFormat => ActionDate.HasValue ? ActionDate.Value.ToString("dd-MM-yyy hh:mm tt") : "";
        public List<string> RejectReason {  get; set; } = new List<string>();
    }

    public class SrutinyRejectModel
    {
        public long? Id { get; set; }
        public string Reason { get; set; } = "";
    }
}
