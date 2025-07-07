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
        services.AddTransient<ILoginDetailsService, LoginDetailsService>();
        services.AddTransient<IProfileService, ProfileService>();
        services.AddTransient<ICurrentCourseService, CurrentCourseService>();
        services.AddTransient<IPastCourseService, PastCourseService>();
        services.AddTransient<IErrorLogger, ErrorLogger>();
        services.AddTransient<INoticeService, NoticeService>();
    }
}
