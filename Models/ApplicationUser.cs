using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystemJSE.Models
{
    public class ApplicationUser : IdentityUser
{
    public string? EmployeeId { get; set; }
    public string? TeamName { get; set; }

    // Make ManagerId nullable
    public string? ManagerId { get; set; }
}

}