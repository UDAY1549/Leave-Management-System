using LeaveManagementSystemJSE.Models;

namespace LeaveManagementSystemJSE.Interfaces
{
    public interface ILeaveRequestRepository : IRepository<LeaveRequest>
    {
        Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(string userId);
        Task<IEnumerable<LeaveRequest>> GetPendingWithUserAsync(); // For Manager Dashboard
    }
}