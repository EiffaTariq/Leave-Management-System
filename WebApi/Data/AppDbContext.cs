using Microsoft.EntityFrameworkCore;
using Shared.Entities;
namespace WebApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<UserLeaveBalance> UserLeaveBalances { get; set; }
       
    }
}
