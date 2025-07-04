using LeaveManagementSystemJSE.Data;
using LeaveManagementSystemJSE.Interfaces;
using LeaveManagementSystemJSE.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemJSE.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext _context;

        public LeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await _context.LeaveRequests.Include(l => l.ApplicationUser).ToListAsync();
        }

        public async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests.Include(l => l.ApplicationUser)
                                               .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(LeaveRequest entity)
        {
            await _context.LeaveRequests.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeaveRequest entity)
        {
            _context.LeaveRequests.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);
            if (leave != null)
            {
                _context.LeaveRequests.Remove(leave);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(string userId)
        {
            return await _context.LeaveRequests
                .Where(l => l.ApplicationUserId == userId)
                .Include(l => l.ApplicationUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingWithUserAsync()
        {
            return await _context.LeaveRequests
                .Where(l => l.Status == LeaveStatus.Pending)
                .Include(l => l.ApplicationUser)
                .ToListAsync();
        }

        public void Update(LeaveRequest entity)
{
    _context.LeaveRequests.Update(entity);
}

public void Delete(LeaveRequest entity)
{
    _context.LeaveRequests.Remove(entity);
}

public async Task SaveAsync()
{
    await _context.SaveChangesAsync();
}

    }
}