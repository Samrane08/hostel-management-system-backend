using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Implementation;
using Repository.Interface;

namespace Repository.Configuration;
public static class DependencyInjection
{
    public static void RegisterRepository(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IBaseDapper, VJNTDapperr>();
        services.AddScoped<IBaseDapper, SJSADapperr>();
        services.AddScoped<IDapperResolver, DapperResolver>();
    }
}
