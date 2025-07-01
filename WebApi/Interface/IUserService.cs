using Shared.Entities;

namespace WebApi.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        User? GetUserByEmailAndPassword(string email, string password);
        User? GetUserByUsernameAndPassword(string username, string password);
        List<UserLeaveBalance> GetUserLeaveBalances(int userId);
        //Task<string> RegisterAsync(RegisterDto dto);
        //Task<ServiceResponse> RegisterUserAsync(RegisterDto dto);

        //Task<ServiceResponse> RegisterAsync(RegisterDto dto);
        Task RegisterAsync(User user);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
