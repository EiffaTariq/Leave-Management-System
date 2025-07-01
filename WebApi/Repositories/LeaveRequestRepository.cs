using Shared.Entities;
using WebApi.Data;
using WebApi.Interface;
using Microsoft.EntityFrameworkCore;
namespace WebApi.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext _context;

        public LeaveRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllAsync()
        {
            //return await _context.LeaveRequests
            //    .Include(l => l.User) // if you want user info with each request
            //    .ToListAsync();
            var leaveRequests = await _context.LeaveRequests
             .Select(l => new LeaveRequestDto
             {
                 LeaveId = l.LeaveId,
                 UserId = l.UserId,
                 StartDate = l.StartDate,
                 EndDate = l.EndDate,
                 Reason = l.Reason,
                 Status = l.Status
             })
             .ToListAsync();
            return leaveRequests;
        }

        public async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests
                .Include(l => l.User)
                .FirstOrDefaultAsync(lr => lr.LeaveId == id);
        }

        public async Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.LeaveRequests
                .Where(lr => lr.UserId == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status)
        {
            return await _context.LeaveRequests
                                 .Include(lr => lr.User)
                                 .Where(lr => lr.Status == status)
                                 .ToListAsync();
        }
     

        public async Task<int> CountApprovedLeavesAsync(int userId, int year)
        {
            return await _context.LeaveRequests
                .CountAsync(lr => lr.UserId == userId &&
                                    lr.StartDate.Year == year &&
                                    lr.Status == "Approved");

        }

        public async Task<User?> GetUserByLeaveIdAsync(int leaveId)
        {
            var leave = await _context.LeaveRequests
         .Include(lr => lr.User)
         .FirstOrDefaultAsync(lr => lr.LeaveId == leaveId);

            return leave?.User;
        }
        public async Task AddAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateLeaveStatusAsync(int leaveId, string newStatus, string adminRemarks)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveId);
            if (leaveRequest == null)
            {
                return false;
            }

            leaveRequest.Status = newStatus;
            leaveRequest.AdminRemarks = adminRemarks;

            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ApproveAsync(int leaveId)
        {
            var leave = await _context.LeaveRequests.FindAsync(leaveId);
            if (leave == null) return false;
            if (leave.Status == "Approved") return false;
            var user = await _context.Users.FindAsync(leave.UserId);
            if (user == null) return false;
            if (user.LeavesTakenThisYear >= 20) return false;
            leave.Status = "Approved";
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> IsOverlappingLeaves(int userId, DateTime start, DateTime end)
        //{
        //    return await _context.LeaveRequests
        //        .AnyAsync(lr => lr.UserId == userId &&
        //                         lr.Status != "Rejected" &&
        //                         lr.StartDate <= end &&
        //                         lr.EndDate >= start);
        //}
        public async Task<bool> IsOverlappingLeaves(int userId, DateTime startDate, DateTime endDate)
        {
            var newStart = startDate.Date;
            var newEnd = endDate.Date;

            return await _context.LeaveRequests
                .AnyAsync(lr => lr.UserId == userId &&
                                lr.StartDate.Date <= newEnd &&
                                lr.EndDate.Date >= newStart);
        }

    }
}
