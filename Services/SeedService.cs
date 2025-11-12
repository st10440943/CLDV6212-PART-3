using Microsoft.AspNetCore.Identity;
using ABC_Retail.Models;
using ABC_Retail.Data;
namespace ABC_Retail.Services
{
    public class SeedService
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
        }
    }
}