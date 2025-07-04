using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystemJSE.Models
{
    public enum LeaveType
    {
        Annual,
        Sick
    }

    public enum LeaveStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public class LeaveRequest
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        public LeaveType Type { get; set; }

        public string? ManagerComments { get; set; }
        public string? ManagerId { get; set; }
        public ApplicationUser? Manager { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // ✅ Fix: Use enum instead of string
        [Required]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}