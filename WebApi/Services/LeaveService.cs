using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using WebApi.Interface;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRequestRepository _leaveRepo;
        private readonly IUserRepository _userRepo;

        public LeaveService(ILeaveRequestRepository leaveRepo, IUserRepository userRepo)
        {
            _leaveRepo = leaveRepo;
            _userRepo = userRepo;
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
        //public Task<IEnumerable<LeaveRequestDto>> GetAllAsync() => _leaveRepo.GetAllAsync();

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

        //public async Task<bool> CreateLeaveAsync(LeaveRequestDto leave)
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
            return new ServiceResponse { Success = true, Message = "Leave request submitted successfully" };



            //var user = await _userRepo.GetByIdAsync(leave.UserId);
            //int daysRequested = (leave.EndDate - leave.StartDate).Days + 1;
            //int remainingLeaves = user.AllowedLeavesPerYear - user.LeavesTakenThisYear;

            //if (daysRequested > remainingLeaves)
            //    return false;
            //var leavesThisYear = await _leaveRepo.CountApprovedLeavesAsync(leave.UserId, DateTime.UtcNow.Year);

            //if (leavesThisYear >= 5)
            //{
            //    Console.WriteLine("You have already taken maximum leaves for the year");
            //    return false;
            //}
            //if (!false)
            //{
            //    await _leaveRepo.AddAsync(leave);
            //}
            //return true;

            //mera khud ka


            //var overlapping = await _leaveRepo.IsOverlappingLeaves(leave.UserId, leave.StartDate, leave.EndDate);
            //if (overlapping)
            //{
            //    throw new Exception("Your leave overlaps with an existing leave.");
            //}

            ////var leavesThisYear = await _leaveRepo.CountApprovedLeavesAsync(leave.UserId, DateTime.UtcNow.Year);
            //var user = await _leaveRepo.GetUserByLeaveIdAsync(leave.LeaveId);

            //var ogUser = await _userRepo.GetByIdAsync(user.Id);
            //if (ogUser == null)
            //{
            //    throw new Exception("No such UserId exists");
            //}
            //var leavesThisYear = ogUser.LeavesTakenThisYear;
            //var leaveDuration = (leave.EndDate.Date - leave.StartDate.Date).Days;

            //if (leavesThisYear + leaveDuration > 6)
            //{
            //    throw new Exception("Your leave request exceeds your maximum leave capacity.");
            //}

            //var leaveReq = new LeaveRequest
            //{
            //    UserId = leave.UserId,
            //    Username = (await _userRepo.GetByIdAsync(leave.UserId))?.Username ?? "Unknown",
            //    LeaveTypeId = leave.LeaveTypeId,
            //    StartDate = leave.StartDate,
            //    EndDate = leave.EndDate,
            //    Reason = leave.Reason,
            //    Status = "Pending"
            //};

            //await _leaveRepo.AddAsync(leaveReq);
            //return true;
        }

        public async Task ApproveLeaveAsync(int leaveId, string adminRemarks)
        {
            //var leave = await _leaveRepo.GetByIdAsync(leaveId);

            //if (leave == null)
            //{
            //    throw new Exception("Leave not found.");
            //}

            //var user = await _userRepo.GetByIdAsync(leave.UserId);
            //if (user == null)
            //{
            //    throw new Exception("User not found.");
            //}

            //var leavesThisYear = await _leaveRepo.CountApprovedLeavesAsync(user.Id, DateTime.UtcNow.Year);
            //var leaveDuration = (leave.EndDate.Date - leave.StartDate.Date).Days;

            //if (user.LeavesTakenThisYear + leaveDuration > 5)
            //{
            //    throw new Exception("Your leave request exceeds your maximum leave capacity.");
            //}

            //leave.Status = "Approved";
            //leave.AdminRemarks = adminRemarks;

            //await _leaveRepo.UpdateAsync(leave);

            //user.LeavesTakenThisYear += leaveDuration;

            //await _userRepo.UpdateAsync(user);

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

        public async Task CreateAsync(LeaveRequest leaveRequest)
        {
            await _leaveRepo.AddAsync(leaveRequest);

        }
    }

}
