using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace WebApi.Interface
{
    public interface ILeaveRequestRepository
    {
        Task<IEnumerable<LeaveRequestDto>> GetAllAsync();
        Task<LeaveRequest?> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status);
        Task<bool> UpdateLeaveStatusAsync(int leaveId, string newStatus, string adminRemarks);
   

        Task<int> CountApprovedLeavesAsync(int userId, int year);

        Task<User?> GetUserByLeaveIdAsync(int leaveId);
        Task AddAsync(LeaveRequest leaveRequest);
        Task UpdateAsync(LeaveRequest leaveRequest);
        Task UpdateUserAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ApproveAsync(int leaveId);
        Task<bool> IsOverlappingLeaves(int userId, DateTime start, DateTime end);

    }
}

