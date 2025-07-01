using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace WebApi.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);

        User? GetUserByEmailAndPassword(string email, string password);
        User? GetUserByUsernameAndPassword(string username, string password);
        Task<User?> GetUserByUsernameAndEmailAsync(string username,string email);

        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetUserByLeaveIdAsync(int leaveId);

        List<UserLeaveBalance> GetLeaveBalancesByUserId(int userId);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
