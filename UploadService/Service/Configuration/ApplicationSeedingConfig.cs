using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.EntityFrameworkCore.Extensions;
using Repository.Data;

namespace Service.Configuration;

public static class ApplicationSeedingConfig
{
    public async static Task AddSeed(this IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsMySql())
        {
            context.Database.Migrate();
        }
    }
}
