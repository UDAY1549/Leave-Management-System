using System.Text.Json;
using LeaveManagementSystemJSE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemJSE.SeedData
{
    public static class IdentitySeed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var filePath = Path.Combine(AppContext.BaseDirectory, "SeedData", "users.json");
            var json = await File.ReadAllTextAsync(filePath);
            var seedData = JsonSerializer.Deserialize<SeedDataModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // 1. Create roles
            foreach (var role in seedData.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Create Manager users
            foreach (var user in seedData.Managers)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var manager = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        EmployeeId = user.EmployeeId,
                        TeamName = user.TeamName,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(manager, user.Password);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(manager, user.Role);
                }
            }

            // 3. Create Employee users with ManagerId assigned
            foreach (var user in seedData.Employees)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var employee = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        EmployeeId = user.EmployeeId,
                        TeamName = user.TeamName,
                        EmailConfirmed = true
                    };

                    // Load all users in this team into memory first
                    var usersInTeamList = await userManager.Users
                        .Where(u => u.TeamName == user.TeamName)
                        .ToListAsync();

                    ApplicationUser? manager = null;

                    // Find a manager user in the team
                    foreach (var userInDb in usersInTeamList)
                    {
                        if (await userManager.IsInRoleAsync(userInDb, "Manager"))
                        {
                            manager = userInDb;
                            break;
                        }
                    }

                    employee.ManagerId = manager?.Id;

                    var result = await userManager.CreateAsync(employee, user.Password);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(employee, user.Role);
                }
            }
        }
    }
}