using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using WebApi.Interface;

namespace WebApi.Controllers
{
    [Route("odata/[controller]")]
    public class LeaveRequestsController : ODataController
    {
        private readonly ILeaveService _leaveService;

        public LeaveRequestsController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var leaves = await _leaveService.GetAllAsync(); // Returns IEnumerable<LeaveRequestDto>
            return Ok(leaves);
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            var leave = await _leaveService.GetByIdAsync(key);
            if (leave == null)
                return NotFound();
            return Ok(leave);
        }
    }

}
