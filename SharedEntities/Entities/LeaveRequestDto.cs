using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class LeaveRequestDto
    {
        [Required]
        public int LeaveId { get; set; }
        public string Status { get; set; } = String.Empty;

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndDate { get; set; } = DateTime.Now;

        [Required]
        public string Reason { get; set; } = string.Empty;

        public int UserId { get; set; } 
    }

}
