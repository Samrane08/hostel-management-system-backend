using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Model;

public class DocumentUploadModel
{

    [Required]
    public int DocumentId { get; set; }

    [Required]
    public IFormFile File { get; set; }
    public string? FileKey { get; set; }
}
public class ViewDocumentModel
{
    public int Id { get; set; }
    public string? DocumentName { get; set; }
    public string? Description { get; set; }
    public string? DocumentSize { get; set; }
    public string? Accept { get; set; }
    public bool IsRequired { get; set; }
    public string? FilePath { get; set; }
}
