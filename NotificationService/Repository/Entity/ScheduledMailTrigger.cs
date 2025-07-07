using System.ComponentModel.DataAnnotations;

namespace Repository.Entity;

public class ScheduledMailTrigger
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? To { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string? Attachment { get; set; }
    public DateTime ScheduledAt { get; set; } = DateTime.Now;
    public DateTime? SendAt { get; set; }
    public bool? SendStatus { get; set; }

}
