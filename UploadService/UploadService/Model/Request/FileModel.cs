using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UploadService.Model.Request
{
    public class FileModel
    {
        [Required]
        public IFormFile file { get; set; }
    }
}
