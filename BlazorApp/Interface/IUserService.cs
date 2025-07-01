using Shared.Entities;
using static System.Net.WebRequestMethods;

namespace BlazorApp.Interface
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);

        Task<List<UserLeaveBalance>?> GetUserLeaveBalancesAsync(int userId);
        Task<List<LeaveRequest>?> GetUserLeaveRequestsAsync(int userId);
        //Task<ServiceResponse> RegisterUserAsync(RegisterDto dto);
        Task<string> RegisterUserAsync(RegisterDto model);
        //yeh naya add kia hai typeid walai error k liay
        Task<bool> LoginAsync(LoginDto dto);
    }

}
