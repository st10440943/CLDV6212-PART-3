using ABC_Retail.Data;
using ABC_Retail.Models;
using ABC_Retail.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC
builder.Services.AddControllersWithViews();

// Configure SQL Server DbContext for Identity
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Configure Identity
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Azure Storage services
builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection("StorageOptions"));

var storage = builder.Configuration.GetSection("StorageOptions").Get<StorageOptions>();

builder.Services.AddSingleton(new TableStorageService(storage));
builder.Services.AddSingleton(new BlobStorageService(storage));
builder.Services.AddSingleton(new FileShareStorageService(storage));
builder.Services.AddSingleton(new QueueStorageService(storage.ConnectionString, storage.QueueName));

var app = builder.Build();

// Seed default users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Users>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.SeedUsersAsync(userManager, roleManager);
}

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Session before authentication
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
