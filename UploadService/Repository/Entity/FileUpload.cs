using Repository.Enums;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entity;

public class FileUpload : BaseAuditableEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();   
    public string? FileName { get; set; }
    public string? ContentType { get; set; }  
    public string? CloudeKey { get; set; }
    
}
