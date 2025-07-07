using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Text.Json;
using UploadService.Model.Request;

namespace UploadService.Controllers
{
    public class FileController : APIBaseController
    {
        private readonly IUploadService uploadservice;

        public FileController(IUploadService uploadservice)
        {
            this.uploadservice = uploadservice;
        }

        [HttpGet("{Key}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string Key)
        {
            var result = await uploadservice.GetByIdAsync(Key);
            return Ok(result);           
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] FileModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await uploadservice.UploadAsync(model.file);
            if (result != null && !string.IsNullOrWhiteSpace(result.Id))
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
