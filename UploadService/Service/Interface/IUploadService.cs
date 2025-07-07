using Microsoft.AspNetCore.Http;
using Model;

namespace Service.Interface;

public interface IUploadService
{
    Task<UploadModel> UploadAsync(IFormFile File);
    Task<object> GetByIdAsync(string Id);
}