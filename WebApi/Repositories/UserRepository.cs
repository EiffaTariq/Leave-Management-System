using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Shared.Entities;
using WebApi.Data;
using WebApi.Interface;
namespace WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            //return await _context.Users
            //    .Include(u => u.LeaveRequests)
            //    .Include(u => u.LeaveBalances)
            //    .FirstOrDefaultAsync(u => u.Id == id);
            var user = await _context.Users.FirstOrDefaultAsync(i=>i.Id == id);
            return user;
        }
      
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.LeaveRequests)
                .Include(u => u.LeaveBalances)
                .ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.LeaveRequests)
                .Include(u => u.LeaveBalances)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public User? GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users
                           .FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
       
        public async Task<User?> GetUserByUsernameAndEmailAsync(string username, string email)
        {
            return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
        }
        public List<UserLeaveBalance> GetLeaveBalancesByUserId(int userId)
        {
            return _context.UserLeaveBalances
                .Where(lb => lb.UserId == userId)
                .ToList();
        }

        public async Task<User?> GetUserByLeaveIdAsync(int leaveId)
        {
            var leave = await _context.LeaveRequests
         .Include(lr => lr.User)
         .FirstOrDefaultAsync(lr => lr.LeaveId == leaveId);

            return leave?.User;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}