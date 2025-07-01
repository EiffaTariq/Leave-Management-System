using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shared.Entities
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }


        public string Username { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? AdminRemarks { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
