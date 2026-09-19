using Azure.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using WebApi.Hubs;
using WebApi.Interface;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRequestRepository _leaveRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHubContext<NotificationHub> _hub;

        public LeaveService(ILeaveRequestRepository leaveRepo, IUserRepository userRepo,
            IHubContext<NotificationHub> hub)
        {
            _leaveRepo = leaveRepo;
            _userRepo = userRepo;
            _hub = hub;
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllAsync()
        {
            var leaves = await _leaveRepo.GetAllAsync();
            return leaves.Select(l => new LeaveRequestDto
            {
                LeaveId = l.LeaveId,
                UserId = l.UserId,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason,
                Status = l.Status
            });
        }

        public Task<LeaveRequest?> GetByIdAsync(int id) => _leaveRepo.GetByIdAsync(id);
        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status)
        {
            return await _leaveRepo.GetLeaveRequestsByStatusAsync(status);
        }

        public Task<IEnumerable<LeaveRequest>> GetByUserIdAsync(int userId) => _leaveRepo.GetByUserIdAsync(userId);
        public async Task UpdateUserAsync(User user)
        {
            await _userRepo.UpdateAsync(user);
        }


     
        public async Task<User?> GetUserByLeaveIdAsync(int leaveId)
        {
            return await _leaveRepo.GetUserByLeaveIdAsync(leaveId);
        }

        public async Task<ServiceResponse> CreateLeaveAsync(LeaveRequestDto leave)

        {

            var user = await _userRepo.GetByIdAsync(leave.UserId);
            if (user == null)
            {
                return new ServiceResponse { Success = false, Message = "User not found" };
            }
            int daysRequested = (leave.EndDate - leave.StartDate).Days + 1;
            int remainingLeaves = user.AllowedLeavesPerYear - user.LeavesTakenThisYear;

            if (daysRequested > remainingLeaves)
            {
                return new ServiceResponse { Success = false, Message = "The days you have requested exceed your annual leave limit" };
            }
            var overlapping = await _leaveRepo.IsOverlappingLeaves(leave.UserId, leave.StartDate, leave.EndDate);
            if (overlapping)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "The days you have requested are overlapping with your previous leave request"
                };
            }
            await _userRepo.UpdateAsync(user);
            var leaveReq = new LeaveRequest
            {
                UserId = leave.UserId,
                Username = (await _userRepo.GetByIdAsync(leave.UserId))?.Username ?? "Unknown",
                //LeaveTypeId = leave.LeaveTypeId,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                Status = "Pending"
            };

            await _leaveRepo.AddAsync(leaveReq);
            Console.WriteLine($"Sending notification for {leaveReq.Username}");
            await _hub.Clients.All.SendAsync("ReceiveNotification",
                $"{leaveReq.Username} submitted a leave request");
            return new ServiceResponse { Success = true, Message = "Leave request submitted successfully" };
       

            
        }

        public async Task ApproveLeaveAsync(int leaveId, string adminRemarks)
        {
            
            var leave = await _leaveRepo.GetByIdAsync(leaveId);
            if (leave == null)
            {
                throw new Exception("Leave not found.");
            }

            var user = await _userRepo.GetByIdAsync(leave.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var leavesThisYear = await _leaveRepo.CountApprovedLeavesAsync(user.Id, DateTime.UtcNow.Year);
            var leaveDuration = (leave.EndDate.Date - leave.StartDate.Date).Days + 1;

            if (leavesThisYear + leaveDuration > 10)
            {
                throw new Exception("This leave cannot be approved; maximum annual quota exceeded.");
            }

            leave.Status = "Approved";
            leave.AdminRemarks = adminRemarks;

            await _leaveRepo.UpdateAsync(leave);

            user.LeavesTakenThisYear += leaveDuration;
            await _userRepo.UpdateAsync(user);
        }


        public async Task RejectLeaveAsync(int leaveId, string adminRemarks)
        {
            var leave = await _leaveRepo.GetByIdAsync(leaveId);
            if (leave == null)
            {
                Console.WriteLine($"Leave with id {leaveId} not found.");
                return;
            }

            leave.Status = "Rejected";
            leave.AdminRemarks = adminRemarks;

            await _leaveRepo.UpdateAsync(leave);
            Console.WriteLine($"Leave request {leaveId} successfully updated.");
        }
        public async Task<bool> UpdateLeaveStatusAsync(int leaveId, string newStatus, string adminRemarks)
        {
            return await _leaveRepo.UpdateLeaveStatusAsync(leaveId, newStatus, adminRemarks);
        }

   
    }

}
