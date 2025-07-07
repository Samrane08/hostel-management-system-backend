using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace Service.Interface;

public interface IWFileSaveService
{
    Task<string> WUploadAsync(IFormFile File);
    Task<GetObjectResponse> WGetAsync(string key);
}
