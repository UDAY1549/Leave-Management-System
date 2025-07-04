namespace LeaveManagementSystemJSE.Models
{
    public class SeedUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string EmployeeId { get; set; }
        public string TeamName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class SeedDataModel
    {
        public List<string> Roles { get; set; }
        public List<SeedUser> Managers { get; set; }
        public List<SeedUser> Employees { get; set; }
    }
}