using LeaveManagementSystemJSE.Data;
using LeaveManagementSystemJSE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using LeaveManagementSystemJSE.Interfaces;
using LeaveManagementSystemJSE.Repositories;
using LeaveManagementSystemJSE.SeedData;


var builder = WebApplication.CreateBuilder(args);

// ✅ Register EF Core with MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 33)) // Use your MySQL version
    )
);

// ✅ Add Identity (User + Roles)
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>() // 👈 Enables Role management
.AddEntityFrameworkStores<AppDbContext>();

// ✅ Enable MVC & Razor Pages (required for Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); 
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();// 👈 Needed for /Identity/Account/Login

var app = builder.Build();

// ✅ Seed Roles and Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeed.InitializeAsync(services);
}

// ✅ Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // 👈 Must come before Authorization
app.UseAuthorization();

// ✅ MVC and Razor Pages routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // 👈 Required for Identity UI like /Account/Login

app.Run();