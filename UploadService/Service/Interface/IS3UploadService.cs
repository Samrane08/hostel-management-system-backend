using Microsoft.AspNetCore.Http;

namespace Service.Interface;

public interface IS3UploadService
{
    Task<string> UploadAsync(IFormFile file, string bucketName);
    Task<string> UploadByteArrayAsync(byte[] byteArray, string bucketName, string fileName, string contentType);
    string GetFileUrl(string filePath);
}
