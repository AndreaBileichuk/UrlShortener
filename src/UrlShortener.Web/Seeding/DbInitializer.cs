using Microsoft.AspNetCore.Identity;
using UrlShortener.DAL.Data;
using UrlShortener.DAL.Entities;

namespace UrlShortener.Web.Seeding;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { RoleNames.Admin, RoleNames.User };

        foreach (var role in roles)
        {
            if (!(await roleManager.RoleExistsAsync(role.ToString())))
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }

        var adminEmail = "admin@domain.com";
        var adminUserName = "Admin123";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminUserName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);
            }
        }
        
        // Setting some default context for About page
        var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

        if (!db.AboutContents.Any())
        {
            db.AboutContents.Add(new AboutContent()
            {
                Text = "Наш алгоритм використовує магію Base62 для перетворення ID бази даних у короткі рядки."
            });

            await db.SaveChangesAsync();
        }
    }
}