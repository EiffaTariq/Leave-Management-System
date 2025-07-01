using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class UserLeaveBalance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User ?User { get; set; }

        public int AllowedLeaves { get; set; } = 10;   
        public int LeavesTaken { get; set; }   
        public int Year { get; set; } = DateTime.Now.Year;
    }
}
