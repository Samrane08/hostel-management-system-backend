using System.ComponentModel.DataAnnotations;

namespace Repository.Entity;

public class EmailTemplate:BaseAuditableEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Key { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
}
