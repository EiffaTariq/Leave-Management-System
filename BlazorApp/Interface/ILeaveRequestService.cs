using Shared.Entities;

namespace BlazorApp.Interface
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetUserLeaveRequestsAsync(int userId);
        Task<List<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status);
        //Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequest request);
        //Task<LeaveRequest> CreateLeaveRequestAsync(LeaveRequestDto dto);
        //Task CreateLeaveRequestAsync(LeaveRequestDto request);
        Task<ServiceResponse> CreateLeaveRequestAsync(LeaveRequestDto request);
        Task ApproveLeaveAsync(int id, string adminRemarks);
        Task RejectLeaveAsync(int leaveId, string adminRemarks);
        Task UpdateLeaveStatusAsync(int id, string status, string remarks);
        Task<List<LeaveRequestDto>> LoadApprovedLeavesAsync();
        //Task<HttpResponseMessage> UpdateLeaveStatusAsync(int leaveId, string newStatus);
        Task<List<LeaveRequestDto>> LoadPendingLeavesAsync();

        Task<List<LeaveRequestDto>> LoadLeavesAsync();
        Task<List<LeaveRequestDto>> GetAllLeaveRequestsAsync();
    }

}
