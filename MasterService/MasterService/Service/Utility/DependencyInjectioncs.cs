using MasterService.Service.Interface;

namespace MasterService.Service.Utility
{
    public static class DependencyInjectioncs
    {
        public static void AddService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IErrorLogger, ErrorLogger>();
        }
    }
}
