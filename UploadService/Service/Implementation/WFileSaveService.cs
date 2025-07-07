using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Service.Implementation;
public class WFileSaveService: IWFileSaveService
{
    private readonly AWSOptionW optionsW;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;

    public WFileSaveService(IOptions<AWSOptionW> optionsW,ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.optionsW = optionsW.Value;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }
    public async Task<string> WUploadAsync(IFormFile File)
    {
        try
        {
            string Directory = $"{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}/{currentUserService.UserId}";
            string fileExtension = Path.GetExtension(File.FileName) ?? "";
            string fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
            var FileKey = Directory + "/" + fileName;

            var credentials = new BasicAWSCredentials(optionsW.AccessKey, optionsW.SecretKey);
            var config = new AmazonS3Config
            {
                ServiceURL = optionsW.ServiceUrl,
                ForcePathStyle = true
            };

            //ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            //                                     | SecurityProtocolType.Tls11
            //                                     | SecurityProtocolType.Tls12
            //                                     | SecurityProtocolType.Tls13;

            var _s3Client = new AmazonS3Client(credentials, config);
            var request = new PutObjectRequest()
            {
                BucketName = optionsW.BucketName,
                Key = FileKey,
                InputStream = File.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", File.ContentType);
            await _s3Client.PutObjectAsync(request);
            return FileKey;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("S3 Bucket Upload",ex);
            return string.Empty;
        }        
    }
    public async Task<GetObjectResponse> WGetAsync(string key)
    {
        try
        {
            var credentials = new BasicAWSCredentials(optionsW.AccessKey, optionsW.SecretKey);
            var config = new AmazonS3Config
            {
                ServiceURL = optionsW.ServiceUrl,
                ForcePathStyle = true
            };

            //ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            //                                     | SecurityProtocolType.Tls11
            //                                     | SecurityProtocolType.Tls12
            //                                     | SecurityProtocolType.Tls13;

            var _s3Client = new AmazonS3Client(credentials, config);

            var s3Object = await _s3Client.GetObjectAsync(optionsW.BucketName, key);
            return s3Object;
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("S3 Bucket File Access", ex);
            return new GetObjectResponse();
        }
    }


    private static bool ValidateServerCertificate(
       object sender,
       X509Certificate certificate,
       X509Chain chain,
       SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}
