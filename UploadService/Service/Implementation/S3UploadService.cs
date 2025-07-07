using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model;
using Service.Interface;

namespace Service.Implementation;

public class S3UploadService : IS3UploadService
{
    TransferUtility _awsClient;
    private readonly AWSOptionS3 _fileUploadOptions;
    private readonly ILogger<S3UploadService> _logger;
    private readonly RegionEndpoint regionName;

    public S3UploadService(IOptions<AWSOptionS3> fileUploadOptions,ILogger<S3UploadService> logger)
    {
        _fileUploadOptions = fileUploadOptions.Value;
        _logger = logger;
        regionName = Amazon.RegionEndpoint.GetBySystemName(_fileUploadOptions.RegionName);
        _awsClient = new TransferUtility(_fileUploadOptions.AccessKey, _fileUploadOptions.SecretKey, regionName);
    }
    
    public async Task<string> UploadAsync(IFormFile file, string bucketName)
    {
        try
        {
            bucketName = $"{bucketName}/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}";
            string fileExtension = Path.GetExtension(file.FileName) ?? "";
            string fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
            var filePath = bucketName + "/" + fileName;
            using (var fileAsStreamData = file.OpenReadStream())
            {
                Stream processedFileData;
                if (FileUploadConstants.JpegFormats.Any(x => x.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)))
                    processedFileData = ImageProcessor.ProcessImage(fileAsStreamData);
                else processedFileData = fileAsStreamData;
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = _fileUploadOptions.BucketName,
                    //FilePath = file.,
                    InputStream = processedFileData,
                    StorageClass = S3StorageClass.ReducedRedundancy,
                    //PartSize = //_fileUploadOptions.AWS_PartSize,
                    Key = "public/" + filePath,
                    CannedACL = S3CannedACL.Private
                };
                await _awsClient.UploadAsync(fileTransferUtilityRequest);
                processedFileData.Dispose();
            }
            return filePath;
        }
        catch (AmazonS3Exception e)
        {
            ExceptionLogging.LogException(Convert.ToString(e));
            _logger.LogError(e, "Error While uploading image in AWS");
            return e.Message;
        }
    }
    
    public async Task<string> UploadByteArrayAsync(byte[] byteArray, string bucketName, string fileName, string contentType)
    {
        try
        {
            var stream = new MemoryStream(byteArray);
            bucketName = $"{bucketName}/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}";
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Guid.NewGuid().ToString();
            }
            else
            {
                fileName = $@"{Guid.NewGuid()}_{fileName}";
            }
            var filePath = bucketName + "/" + fileName;
            TransferUtilityUploadRequest fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = _fileUploadOptions.BucketName,
                InputStream = stream,
                StorageClass = S3StorageClass.ReducedRedundancy,
                //PartSize = _fileUploadOptions.AWS_PartSize,
                Key = filePath,
                CannedACL = S3CannedACL.PublicRead
            };
            await _awsClient.UploadAsync(fileTransferUtilityRequest);
            return filePath;
        }
        catch (AmazonS3Exception e)
        {
            ExceptionLogging.LogException(Convert.ToString(e));
            _logger.LogError(e, "Error While uploading image (UploadByteArrayAsync) in AWS");
            return e.Message;
        }
    } 
    public string GetFileUrl(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = string.Empty;
        }
        return Path.Combine(_fileUploadOptions.ImageUrl, filePath).Replace("\\", "/");
    }

    public async Task RemoveFileAsync(string objectName, string bucketName = null)
    {
        try
        {
            // delete test.txt file  
            string filePath = objectName;
            var deleteFileRequest = new DeleteObjectRequest
            {
                BucketName = _fileUploadOptions.BucketName,
                Key = filePath
            };
            DeleteObjectResponse fileDeleteResponse = await _awsClient.S3Client.DeleteObjectAsync(deleteFileRequest);

            // delete sub-folder  
            string folderPath = Path.GetDirectoryName(objectName);
            var deleteFolderRequest = new DeleteObjectRequest
            {
                BucketName = _fileUploadOptions.BucketName,
                Key = folderPath
            };
            DeleteObjectResponse folderDeleteResponse = await _awsClient.S3Client.DeleteObjectAsync(deleteFolderRequest);
        }
        catch (Exception e)
        {
            ExceptionLogging.LogException(Convert.ToString(e));
            _logger.LogError(e, "Error While deleting resource in AWS");
        }
    }

    public static class FileUploadConstants
    {
        public static string[] JpegFormats { get; } = new[] { ".jpg", ".jpeg" };
    }
}
