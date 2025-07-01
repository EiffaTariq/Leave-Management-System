//deepseek class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

        public int AllowedLeavesPerYear { get; set; } = 20;
        public int LeavesTakenThisYear { get; set; } = 0;
        public int Year { get; set; } = DateTime.Now.Year;

        public List<UserLeaveBalanceDto>? LeaveBalances { get; set; }
    }
}
