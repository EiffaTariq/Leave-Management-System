using Shared.Entities;

namespace WebApi.Interface
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequestDto>> GetAllAsync();
        Task<LeaveRequest?> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status);
    

        Task<User?> GetUserByLeaveIdAsync(int leaveId);
        Task UpdateUserAsync(User user);
        Task<bool> UpdateLeaveStatusAsync(int leaveId, string newStatus, string adminRemarks);

        //Task<bool> CreateLeaveAsync(LeaveRequestDto leave);
        Task<ServiceResponse> CreateLeaveAsync(LeaveRequestDto leave);

        Task ApproveLeaveAsync(int leaveId, string adminRemarks);
        Task RejectLeaveAsync(int leaveId, string adminRemarks);
        Task CreateAsync(LeaveRequest leaveRequest);
    }

}
