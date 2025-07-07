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
        services.AddTransient<IUIDService, UIDService>();
    }
}
