using Microsoft.AspNetCore.Identity;
using ProfileApp.Models;

namespace ProfileApp.Data;

public static class IdentityInitializer {

  public static void SeedData(IServiceProvider serviceProvider) {
    /*
     * seed roller
     */
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = ["User", "Admin", "Owner"];

    foreach (string roleName in roleNames)
      if (!roleManager.RoleExistsAsync(roleName).Result)
        roleManager.CreateAsync(new IdentityRole(roleName)).Wait();

    /*
     * seed brugere
     */
    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

    const string adminEmail = "admin@example.dk";
    const string adminPassword = "Abc12345";

    AppUser? admin = userManager.FindByNameAsync(adminEmail).Result;

    if (admin is null) {
      admin = new AppUser {
        UserName = adminEmail,
        Email = adminEmail
      };

      IdentityResult result = userManager.CreateAsync(admin, adminPassword).Result;

      if (!result.Succeeded)
        throw new InvalidOperationException(
          // sammensætter fejlbeskeder med mellemrum
          string.Join(" ", result.Errors.Select(e => e.Description))
        );
    }

    string[] adminRoles = ["User", "Admin"];

    foreach (string roleName in adminRoles)
      if (!userManager.IsInRoleAsync(admin, roleName).Result)
        userManager.AddToRoleAsync(admin, roleName).Wait();
  }
}
