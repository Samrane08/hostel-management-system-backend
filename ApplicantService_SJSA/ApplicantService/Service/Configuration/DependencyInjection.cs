using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Repository.Configuration;
using Service.Implementation;
using Service.Interface;

namespace Service.Configuration;
public static class DependencyInjection
{
    public static void AddService(this IServiceCollection services, IConfiguration configuration)
    {
        //test commit
        services.RegisterRepository(configuration);
        services.AddTransient<ILoginDetailsService, LoginDetailsService>();
        services.AddTransient<IProfileService, ProfileService>();
        services.AddTransient<ICurrentCourseService, CurrentCourseService>();
       
        services.AddTransient<IBankAccount,BankAccountservice>();
        services.AddTransient<IPastCourseService, PastCourseService>();
        services.AddTransient<IErrorLogger, ErrorLogger>();

        services.AddTransient<INoticeService, NoticeService>();
        services.AddTransient<IAttendanceDetails, AttendanceDeatilsService>();

    }
}
