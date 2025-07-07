using System.ComponentModel.DataAnnotations;

namespace Repository.Entity;

public class SMSTemplate: BaseAuditableEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? TemplateId { get; set; }
    public string? Key { get; set; }   
    public string? Content { get; set; }
}
