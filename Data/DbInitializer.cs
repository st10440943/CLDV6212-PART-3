using ABC_Retail.Models;
using Microsoft.AspNetCore.Identity;

namespace ABC_Retail.Data
{
    public static class DbInitializer
    {
        public static async Task SeedUsersAsync(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create roles if they don't exist
            var roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

           
            // Seed Customer1
            if (await userManager.FindByEmailAsync("Customer1@test.com") == null)
            {
                var customer1 = new Users
                {
                    UserName = "Customer1@test.com",
                    Email = "Customer1@test.com",
                    Role = "Customer"
                };
                await userManager.CreateAsync(customer1, "Password123");
                await userManager.AddToRoleAsync(customer1, "Customer");
            }

            // Seed Customer2
            if (await userManager.FindByEmailAsync("Customer2@test.com") == null)
            {
                var customer2 = new Users
                {
                    UserName = "Customer2@test.com",
                    Email = "Customer2@test.com",
                    Role = "Customer"
                };
                await userManager.CreateAsync(customer2, "Password123");
                await userManager.AddToRoleAsync(customer2, "Customer");
            }
        }
    }
}
