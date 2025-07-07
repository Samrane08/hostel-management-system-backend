namespace Model
{
    public class ResponseNoticeModel
    {
        public long? Id { get; set; }        
        public string NotificationText { get; set; } = default!;
        public string NotificationHeader { get; set; } = default!;
        public string DocumentId { get; set; } = default!;       
        public DateTime CreatedOn { get; set; }        
    }
    public class ReqNoticeModel
    {
      
        public string NotificationText { get; set; } = default!;
        public string NotificationHeader { get; set; } = default!;
        public string DocumentId { get; set; } = default!;

    }
}
