using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class UserLoginResult
    {
        public int UserId { get; set; }
        public string ?Username { get; set; }
        public string ?Email { get; set; }
        public string ?Role { get; set; }
    }
}
