using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace NotificationService.Service;
public class ConconcurrentJob:IJob
{
    private readonly ILogger<ConconcurrentJob> _logger;
    private static int _counter = 0;
    private readonly IHubContext<NotifyHub> _hubContext;

    public ConconcurrentJob(ILogger<ConconcurrentJob> logger,
        IHubContext<NotifyHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var count = _counter++;

        var beginMessage = $"Conconcurrent Job BEGIN {count} {DateTime.Now}";
        //await _hubContext.Clients.All.SendAsync("ConcurrentJobs", beginMessage);
        _logger.LogInformation(beginMessage);
        Thread.Sleep(7000);

        var endMessage = $"Conconcurrent Job END {count} {DateTime.Now}";
        //await _hubContext.Clients.All.SendAsync("ConcurrentJobs", endMessage);
        _logger.LogInformation(endMessage);
    }
}
