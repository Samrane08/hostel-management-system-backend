using HostelService.Helper;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace HostelService.Service;
public interface IHttpClientService
{
    
    Task<T> RequestSend<T>(HttpMethod requestMethod, string requestUri,dynamic requestBody);
    Task<string> RequestSendToGetFile<T>(HttpMethod requestMethod, string requestUri, dynamic requestBody);
}

public class HttpClientService : IHttpClientService
{
    private readonly APIUrl urloptions;
    public HttpClientService(IOptions<APIUrl> urloptions)
    {
        this.urloptions = urloptions.Value;
    }

    public async Task<T> RequestSend<T>(HttpMethod requestMethod, string requestUri, dynamic requestBody)
    {
        HttpStatusCode HttpStatusCode;
        try
        {
            var request = new HttpRequestMessage(requestMethod, requestUri);
            if (requestBody != null)
            {
                string content = JsonSerializer.Serialize(requestBody);
                request.Content = new StringContent(content, null, "application/json");
            }
            HttpClient httpClient = new HttpClient();
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
    public async Task<string> RequestSendToGetFile<T>(HttpMethod requestMethod, string requestUri, dynamic requestBody)
    {
        HttpStatusCode HttpStatusCode;
        try
        {
            var request = new HttpRequestMessage(requestMethod, requestUri);
            if (requestBody != null)
            {
                string content = JsonSerializer.Serialize(requestBody);
                request.Content = new StringContent(content, null, "application/json");
            }
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            HttpStatusCode = response.StatusCode;
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result.ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
