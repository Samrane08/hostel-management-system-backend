using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Implementation;

public class HttpClientService : IHttpClientService
{
    private readonly APIUrl urloptions;
    //private readonly IErrorLogger errorLogger;
    public HttpClientService(IOptions<APIUrl> urloptions)
    {
        this.urloptions = urloptions.Value;
      //  this.errorLogger = errorLogger;
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

    public async Task<T> RequestSend<T>(HttpMethod requestMethod, string requestUri, dynamic requestBody)
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
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
   
   

