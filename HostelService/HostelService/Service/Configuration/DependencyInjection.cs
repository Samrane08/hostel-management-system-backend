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
        services.AddTransient<IHostelProfileService, HostelProfileService>();
        services.AddTransient<IWardenProfileService, WardenProfileService>();
        services.AddTransient<IDepartmentProfileService, DepartmentProfileService>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ISetVacencyService, SetVacencyService>();        
        services.AddTransient<IHostelDashboardService, HostelDashboardServices>();
        services.AddTransient<IApplicationService, ApplicationService>();
        services.AddTransient<IScrutinyService, ScrutinyService>();
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<IAllotmentService, AllotmentService>();
        services.AddTransient<IAdmissionService, AdmissionService>();
        services.AddTransient<ILotteryService, LotteryService>();
        services.AddTransient<IErrorLogger, ErrorLogger>();
        services.AddTransient<IAdminUtilityService, AdminUtilityService>();
        services.AddTransient<IBenefitAllowanceService, BenefitAllowanceService>();
        services.AddTransient<IOfflineRegistrationService, OfflineRegistrationService>();
        services.AddTransient<IAdmittedRegistrationService, AdmittedRegistrationService>();
        services.AddTransient<IDirectWardenService, DirectWardenService>();
        services.AddTransient<IAttendanceDetailsUploadedByApplicantService, AttendanceDetailsUploadedByApplicantService>();
        
    }
}