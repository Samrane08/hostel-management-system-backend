using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicantService.Service
{
    public class BankBackgroundService : BackgroundService
    {
        private readonly ILogger<BankBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly CancellationTokenSource _cts = new();
        private TaskCompletionSource<bool> _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private bool _isTriggered = false;

        public BankBackgroundService(ILogger<BankBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Method to trigger the background service execution.
        /// </summary>
        public void TriggerTask()
        {
            try
            {
                if (_isTriggered)
                {
                    _logger.LogWarning("Task is already running.");
                    return;
                }

                _logger.LogInformation("Triggering background service...");
                _isTriggered = true;

                // Reset TaskCompletionSource to wake up the ExecuteAsync loop
                if (_tcs.Task.IsCompleted)
                {
                    _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                }

                _tcs.TrySetResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TriggerTask()");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BankBackgroundService started and waiting for trigger...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _tcs.Task; // Wait for the task to be triggered
                    if (stoppingToken.IsCancellationRequested) break;

                    _logger.LogInformation("Executing background task...");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var bankService = scope.ServiceProvider.GetRequiredService<IBankAccount>();
                        await bankService.CallBankBangroundService();
                    }

                    _logger.LogInformation("Background task completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background service execution.");
                }
                finally
                {
                    _isTriggered = false; // Reset trigger
                    _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping BankBackgroundService...");
            _cts.Cancel();
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _cts.Dispose();
            base.Dispose();
        }
    }
}
