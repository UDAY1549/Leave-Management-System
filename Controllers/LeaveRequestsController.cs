using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystemJSE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LeaveManagementSystemJSE.Interfaces;
using LeaveManagementSystemJSE.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementSystemJSE.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly AppDbContext _context;

        public LeaveRequestsController(
            ILeaveRequestRepository leaveRequestRepo,
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var requests = await _context.LeaveRequests
                .Include(lr => lr.ApplicationUser)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var leaveRequest = await _leaveRequestRepo.GetByIdAsync(id.Value);
            if (leaveRequest == null) return NotFound();
            return View(leaveRequest);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                leaveRequest.ApplicationUserId = _userManager.GetUserId(User);
                leaveRequest.Status = LeaveStatus.Pending;
                await _leaveRequestRepo.AddAsync(leaveRequest);
                await _leaveRequestRepo.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequest);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var leaveRequest = await _leaveRequestRepo.GetByIdAsync(id.Value);
            if (leaveRequest == null) return NotFound();
            return View(leaveRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _leaveRequestRepo.Update(leaveRequest);
                    await _leaveRequestRepo.SaveAsync();
                }
                catch
                {
                    return Problem("Unable to save changes.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveRequest);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var leaveRequest = await _leaveRequestRepo.GetByIdAsync(id.Value);
            if (leaveRequest == null) return NotFound();
            return View(leaveRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest = await _leaveRequestRepo.GetByIdAsync(id);
            if (leaveRequest != null)
            {
                _leaveRequestRepo.Delete(leaveRequest);
                await _leaveRequestRepo.SaveAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Approve(int id)
        {
            var currentManager = await _userManager.GetUserAsync(User);
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.ApplicationUser)
                .FirstOrDefaultAsync(lr => lr.Id == id);

            if (leaveRequest == null) return NotFound();

            // Prevent managers from approving their own requests or requests not under them
            if (leaveRequest.ApplicationUserId == currentManager.Id ||
                leaveRequest.ApplicationUser.ManagerId != currentManager.Id)
            {
                return Forbid();
            }

            leaveRequest.Status = LeaveStatus.Approved;
            leaveRequest.ManagerId = currentManager.Id;

            _leaveRequestRepo.Update(leaveRequest);
            await _leaveRequestRepo.SaveAsync();

            return RedirectToAction(nameof(ManagerDashboard));
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            var currentManager = await _userManager.GetUserAsync(User);
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.ApplicationUser)
                .FirstOrDefaultAsync(lr => lr.Id == id);

            if (leaveRequest == null) return NotFound();

            if (leaveRequest.ApplicationUserId == currentManager.Id ||
                leaveRequest.ApplicationUser.ManagerId != currentManager.Id)
            {
                return Forbid();
            }

            leaveRequest.Status = LeaveStatus.Rejected;
            leaveRequest.ManagerId = currentManager.Id;

            _leaveRequestRepo.Update(leaveRequest);
            await _leaveRequestRepo.SaveAsync();

            return RedirectToAction(nameof(ManagerDashboard));
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerDashboard()
        {
            var manager = await _userManager.GetUserAsync(User);

            var pendingRequests = await _context.LeaveRequests
                .Include(lr => lr.ApplicationUser)
                .Where(lr =>
                    lr.Status == LeaveStatus.Pending &&
                    lr.ApplicationUser.ManagerId == manager.Id &&
                    lr.ApplicationUserId != manager.Id)
                .ToListAsync();

            return View(pendingRequests);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            var managerIds = managers.Select(m => m.Id).ToList();

            var requests = await _context.LeaveRequests
                .Include(lr => lr.ApplicationUser)
                .Where(lr => managerIds.Contains(lr.ApplicationUserId))
                .ToListAsync();

            return View(requests);
        }
    }
}