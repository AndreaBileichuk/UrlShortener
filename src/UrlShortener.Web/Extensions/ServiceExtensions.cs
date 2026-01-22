using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.Services.AboutContent;
using UrlShortener.DAL.Data;
using UrlShortener.DAL.Entities;

namespace UrlShortener.Web.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddLowercaseRouting(this IServiceCollection services)
    {
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, b => { b.MigrationsAssembly("UrlShortener.DAL"); });
        });

        return services;
    }

    public static IServiceCollection AddConfigOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAboutContentService, AboutContentService>();
        
        return services;
    }
}