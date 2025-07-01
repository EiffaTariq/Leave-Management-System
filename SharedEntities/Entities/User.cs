using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Shared.Entities
{
    public class User
    {
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string Email { get; set; } = string.Empty;

    public int AllowedLeavesPerYear { get; set; } = 10;
    public int LeavesTakenThisYear { get; set; } = 0;
    public int Year { get; set; } = DateTime.Now.Year;

    [JsonIgnore]
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    [JsonIgnore]
    public ICollection<UserLeaveBalance> LeaveBalances { get; set; } = new List<UserLeaveBalance>();
    }
}
