using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Authorize] //yt
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly IUserService _userService;
        public LeaveRequestController(ILeaveService leaveService, IUserService userService)
        {
            _leaveService = leaveService;
            _userService = userService;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestDto request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    await _leaveService.CreateLeaveRequestAsync(request);
        //    return Ok();
        //}
        [HttpPost("create")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestDto requestDto)
        {

            if (requestDto == null)
                return BadRequest(new ServiceResponse { Success = false, Message = "Invalid leave request." });

            var user = await _userService.GetByIdAsync(requestDto.UserId);
            if (user == null)
                return BadRequest(new ServiceResponse { Success = false, Message = "User not found." });

            var result = await _leaveService.CreateLeaveAsync(requestDto);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }



            //    if (!ModelState.IsValid)
            //        return BadRequest(ModelState);

            //    await _leaveService.CreateLeaveRequestAsync(requestDto);
            //    return Ok(new { message = "Leave request submitted successfully." });
            //if (requestDto == null)
            //    return BadRequest("Invalid leave request.");

            //var user = await _userService.GetByIdAsync(requestDto.UserId);
            //if (user == null)
            //    return BadRequest("User not found.");

            //var leaveRequest = new LeaveRequest
            //{
            //    UserId = requestDto.UserId,
            //    StartDate = requestDto.StartDate,
            //    EndDate = requestDto.EndDate,
            //    Reason = requestDto.Reason,
            //    Status = "Pending" // or whatever your default is
            //};

            //await _leaveService.CreateAsync(leaveRequest);
            //return Ok("Leave request successfully created.");


            //if (requestDto == null)
            //    return BadRequest("Invalid leave request.");

            //var user = await _userService.GetByIdAsync(requestDto.UserId);
            //if (user == null)
            //    return BadRequest("User not found.");


        }




        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> SubmitLeaveRequest([FromBody] LeaveRequest leaveRequest)
        //{
        //    var result = await _leaveService.RequestLeaveAsync(leaveRequest);
        //    if (result == null) return BadRequest("Leave limit exceeded or invalid data.");
        //    return Ok("Leave request submitted.");
        //}

        //[HttpGet("user/{userId}")]
        //[Authorize]
        //public async Task<IActionResult> GetUserLeaveRequests(int userId)
        //{
        //    var leaves = await _leaveService.GetByUserIdAsync(userId);
        //    return Ok(leaves);
        //}

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLeaveRequestsByUserId(int userId)
        {
            try
            {
                var requests = await _leaveService.GetByUserIdAsync(userId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLeaveRequestsByStatus(string status)
        {
            var leaves = await _leaveService.GetLeaveRequestsByStatusAsync(status);
            return Ok(leaves);
        }

        // Approve a leave request
        //[HttpPut("approve/{leaveId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> ApproveLeaveRequest(int leaveId)
        //{
        //    var result = await _leaveService.UpdateLeaveStatusAsync(leaveId, "Approved", "Approved by Admin");
        //    if (!result) return NotFound("Leave request not found.");
        //    return Ok("Leave request approved.");
        //}

        //[HttpPut("reject/{leaveId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> RejectLeaveRequest(int leaveId, [FromBody] string adminRemarks)
        //{
        //    var result = await _leaveService.UpdateLeaveStatusAsync(leaveId, "Rejected", adminRemarks);
        //    if (!result) return NotFound("Leave request not found.");
        //    return Ok("Leave request rejected.");
        //}
    }
}
