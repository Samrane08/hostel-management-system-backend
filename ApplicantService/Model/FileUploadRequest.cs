using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
