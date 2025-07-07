using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Configuration;
using Service.Implementation;
using Service.Interface;

namespace Service.Configuration;
public static class DependencyInjection
{
    public static void AddService(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterRepository(configuration);
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ISMSService, SMSService>();
        services.AddTransient<IEmailLogger, EmailLogger>();
        services.AddTransient<ISMSLogger, SMSLogger>();
        services.AddTransient<IOtpVerifyService, OtpVerifyService>();
        services.AddTransient<IEmailTemplateService, EmailTemplateService>();
        services.AddTransient<IExceptionLogger, ExceptionLogger>();
        services.AddTransient<IScheduledMailTriggerService, ScheduledMailTriggerService>();
    }
}
