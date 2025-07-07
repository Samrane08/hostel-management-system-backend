using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Model.Common;
using System.Text.Json;

namespace NotificationService.Service;
public class SignalRClientMiddleware
{
    private readonly RequestDelegate _next;
    private readonly EnvironmentSetting options;
    private readonly HubConnection _hubConnection;
    private string Uri = "https://localhost:7034";
    public SignalRClientMiddleware(RequestDelegate next,IOptions<EnvironmentSetting> options)
    {
        _next = next;
        this.options = options.Value;

        if (this.options != null && this.options.Environment == "UAT")        
            Uri = this.options.UATUrl;
        else if(this.options.Environment == "PROD")
            Uri = this.options.ProdUrl;

        _hubConnection = new HubConnectionBuilder()
               .WithUrl($"{Uri}/Notify", options =>
               {
                   options.SkipNegotiation = true;
                   options.Transports = HttpTransportType.WebSockets;
               }).WithAutomaticReconnect().Build();

        _hubConnection.StartAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Console.WriteLine("Error starting SignalR connection: " + task.Exception.GetBaseException());
            }
            else
            {
                Console.WriteLine("SignalR connection started.");
            }
        });

        _hubConnection.On<string, object>("EmailNotification", async (trackid, data) =>
        {
            Console.WriteLine($"{trackid} :{JsonSerializer.Serialize(data)}");

            if (!string.IsNullOrWhiteSpace(trackid))
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, $"{Uri}/notification-service/api/email/brodcast");
                    string content = JsonSerializer.Serialize(trackid);
                    request.Content = new StringContent(content, null, "application/json");
                    await httpClient.SendAsync(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{trackid} : Error :{ex.Message}");
                }
            }
        });

        _hubConnection.On<string, object>("SMSNotification", async (trackid, data) =>
        {
            Console.WriteLine($"{trackid} :{JsonSerializer.Serialize(data)}");

            if (!string.IsNullOrWhiteSpace(trackid))
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, $"{Uri}/notification-service/api/SMS/brodcast");
                    string content = JsonSerializer.Serialize(trackid);
                    request.Content = new StringContent(content, null, "application/json");
                    await httpClient.SendAsync(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{trackid} : Error :{ex.Message}");
                }
            }
        });
    }
    public async Task Invoke(HttpContext context)
    {
        await _next(context);
    }
}
