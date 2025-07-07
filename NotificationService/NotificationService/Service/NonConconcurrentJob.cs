using Microsoft.AspNetCore.SignalR;
using Quartz;
using Service.Interface;

namespace NotificationService.Service
{
    [DisallowConcurrentExecution]
    public class NonConconcurrentJob:IJob
    {
        private readonly ILogger<NonConconcurrentJob> _logger;
        private static int _counter = 0;
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly IScheduledMailTriggerService emailLogger;

        public NonConconcurrentJob(ILogger<NonConconcurrentJob> logger,
                                   IHubContext<NotifyHub> hubContext,
                                   IScheduledMailTriggerService emailLogger)
        {
            _logger = logger;
            _hubContext = hubContext;
            this.emailLogger = emailLogger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var count = _counter++;
            var beginMessage = $"NonConconcurrentJob Job BEGIN {count} {DateTime.UtcNow}";
            
            await emailLogger.ScheduledMailTrigger();

            Thread.Sleep(7000);
            var endMessage = $"NonConconcurrentJob Job END {count} {DateTime.UtcNow}";         
            _logger.LogInformation(endMessage);
        }
    }
}
