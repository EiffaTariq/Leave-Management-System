using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class UserLeaveBalanceDto
    {
        public int AllowedLeaves { get; set; }
        public int LeavesTaken { get; set; }
    }
}
