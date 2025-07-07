using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Data;

namespace Repository.Configuration;
public static class DependencyInjection
{
    public static void RegisterRepository(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySQL(
                   configuration.GetConnectionString("DefaultConnection")));       
    }
}
