using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using WebApi.Interface;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILeaveService _leaveService;

        public AdminController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("approve/{leaveId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveLeave(int leaveId, [FromBody] /*string adminRemarks*/RequestDto request)
        {
            try
            {
                var adminRemarks = request.AdminRemarks;
                await _leaveService.ApproveLeaveAsync(leaveId, adminRemarks);
                var user = await _leaveService.GetUserByLeaveIdAsync(leaveId);
               
                return Ok(new { message = "Leave request approved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("reject/{leaveId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectLeave(int leaveId, [FromBody] RequestDto model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid request.");
                }

                await _leaveService.RejectLeaveAsync(leaveId, model.AdminRemarks);
                return Ok(new { message = "Leave request rejected successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("pending-leaves")]
        public async Task<IActionResult> GetPendingLeaves()
        {
            var pendingLeaves = await _leaveService.GetLeaveRequestsByStatusAsync("Pending");
            return Ok(pendingLeaves);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLeaves()
        {
            var allLeaves = await _leaveService.GetAllAsync();
            if (allLeaves == null || !allLeaves.Any())
            {
                return NotFound("No leave requests found.");
            }
            return Ok(allLeaves);
        }
    }
}
