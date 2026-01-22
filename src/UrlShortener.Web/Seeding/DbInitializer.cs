using Microsoft.AspNetCore.Identity;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Enums;

namespace UrlShortener.Web.Seeding;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        ERoleTypes[] roles = { ERoleTypes.User, ERoleTypes.Admin };

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
                await userManager.AddToRoleAsync(adminUser, ERoleTypes.Admin.ToString());
            }
        }
    }
}