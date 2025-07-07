using System.ComponentModel.DataAnnotations;

namespace Repository.Entity;

public class SMSSender
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? TemplateId { get; set; }
    public string? To { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime? SendAt { get; set; }
    public bool? SendStatus { get; set; }
}
