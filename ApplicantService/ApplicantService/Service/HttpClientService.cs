using ApplicantService.Helper;
using ApplicantService.Model;
using Google.Protobuf.WellKnownTypes;
using Helper;
using Microsoft.Extensions.Options;
using Model;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using Service.Interface;
using System.Net;
using System.Text.Json;

namespace ApplicantService.Service;
public interface IHttpClientService
{
    Task<UploadResponseModel> FileUpload(IFormFile file);
    Task<T> RequestSend<T>(HttpMethod requestMethod, string requestUri,dynamic requestBody);
    Task<T> RequestSendWithHeader<T>(HttpMethod requestMethod, string requestUri, dynamic requestBody);
}

public class HttpClientService: IHttpClientService
{
    private readonly APIUrl urloptions;
    private readonly IErrorLogger errorLogger;
    public HttpClientService(IOptions<APIUrl> urloptions, IErrorLogger errorLogger)
    {       
        this.urloptions = urloptions.Value;
        this.errorLogger = errorLogger;
    }
    public async Task<UploadResponseModel> FileUpload(IFormFile file)
    {
        var model = new UploadResponseModel();
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{urloptions.UploadService}/File");
             var content = new MultipartFormDataContent();
            var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.FileName);
            request.Content = content;
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            model = JsonSerializer.Deserialize<UploadResponseModel>(result);
        }
        catch (Exception ex)
        {

        }
        return model;
    }
    public async Task<T> RequestSend<T>(HttpMethod requestMethod, string requestUri,dynamic requestBody)
    {
        HttpStatusCode HttpStatusCode;
		
		if (string.IsNullOrEmpty(requestUri))
		{
            //ExceptionLogging.LogException(Convert.ToString("Url is null or empty"));
            HttpStatusCode = HttpStatusCode.BadRequest;
            return default(T);
        }
        
		try
		{
            

            var request = new HttpRequestMessage(requestMethod, requestUri);
            if (requestBody != null)
            {
                string content = JsonSerializer.Serialize(requestBody);
                request.Content = new StringContent(content, null, "application/json");
            }
            HttpClient httpClient = new HttpClient();
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            var response = await httpClient.SendAsync(request);
            HttpStatusCode = response.StatusCode;
            response.EnsureSuccessStatusCode();
            var result =  await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<T> RequestSendWithHeader<T>(HttpMethod requestMethod, string requestUri, dynamic requestBody)
    {
       
        HttpStatusCode httpStatusCode;
        string token = "";
		
		 if (string.IsNullOrEmpty(requestUri))
        {
            //ExceptionLogging.LogException(Convert.ToString("Url is null or empty"));
            httpStatusCode = HttpStatusCode.BadRequest;
            return default(T);
        }
		try
        {
            Uri formatedUrl = new Uri(requestUri, UriKind.Absolute);
            var loginModel = new PostLoginModel();
            token = await GetTokenAsync(HttpMethod.Post, $"{urloptions.PostService}/login/login", loginModel);
            var request = new HttpRequestMessage(requestMethod, formatedUrl);
           if (!string.IsNullOrEmpty(token))
            {
              if (requestBody != null)
                {
                    string content = JsonSerializer.Serialize(requestBody);
                    request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                }
                request.Headers.Add("Token", token);
                request.Headers.Add("ClientID", "UmangLive");
                request.Headers.Add("ClientSecret", "43D6BFA7-DA58-4C00-919E-AEDEAEEFEE0E");
                using (HttpClient httpClient = new HttpClient())
                {
                   
                    var response = await httpClient.SendAsync(request);
                    httpStatusCode = response.StatusCode;
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(result);
                }
            }
            else
            {

                return default(T);
            }
        }
        catch (Exception ex)
        {
            await errorLogger.Log("PostApiError", ex);
            return default(T);
        }
    }
    public async Task<string> GetTokenAsync(HttpMethod requestMethod, string requestUri, dynamic requestBody)
    {
        HttpStatusCode httpStatusCode;
        try
        {
            Uri formatedUrl = new Uri(requestUri, UriKind.Absolute);

            var request = new HttpRequestMessage(requestMethod, formatedUrl);
            if (requestBody != null)
            {
                string content = JsonSerializer.Serialize(requestBody);
                request.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            }

            request.Headers.Add("ClientID", "UmangLive");
            request.Headers.Add("ClientSecret", "43D6BFA7-DA58-4C00-919E-AEDEAEEFEE0E");
           // request.Headers.Add("Content-Type", "application/json");

            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request);
                httpStatusCode = response.StatusCode;
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<ApiResponse>(result);

                return jsonResponse.user.Token;
            }
        }
        catch (Exception ex)
        {
            await errorLogger.Log("PostApiCallToken", ex);
            return null;
        }



    }
}
