//With JWTToken
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interface;
using Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApi.Repositories;
namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            


            if (string.IsNullOrWhiteSpace(registerModel.Email) ||
       !registerModel.Email.Contains("@gmail.com"))
            {
                return BadRequest("Valid email is required.");
            }

            if (string.IsNullOrWhiteSpace(registerModel.Password) ||
                registerModel.Password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters.");
            }

            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var previousUser = await _userService.GetUserByEmailAsync(registerModel.Email);
            if (previousUser != null)
            {
                return BadRequest("Email already exists.");
            }

            var adminEmails = new List<string> { "eiffa@gmail.com", "admin@example.com" };
            var role = registerModel.Role;
            //var role = adminEmails.Contains(registerModel.Email.ToLower()) ? "Admin" : "User";

            var newUser = new User
            {
                Username = registerModel.Username,
                Email = registerModel.Email,
                Password = registerModel.Password, // Ideally hashed by UserService
                Role = role
            };

            await _userService.RegisterAsync(newUser);

            // newUser should now have its Id set by EF Core
            var token = JwtTokenHelper.GenerateToken(newUser);

            return Ok(new
            {
                Token = token,
                UserId = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Role = newUser.Role
            });
            //    if (string.IsNullOrWhiteSpace(registerModel.Email) ||
            //!registerModel.Email.Contains("@"))
            //    {
            //        return BadRequest("Valid email is required.");
            //    }

            //    if (string.IsNullOrWhiteSpace(registerModel.Password) ||
            //        registerModel.Password.Length < 6)
            //    {
            //        return BadRequest("Password must be at least 6 characters.");
            //    }

            //    if (registerModel.Password != registerModel.ConfirmPassword)
            //    {
            //        return BadRequest("Passwords do not match.");
            //    }

            //    var previousUser = await _userService.GetUserByEmailAsync(registerModel.Email);
            //    if (previousUser != null)
            //    {
            //        return BadRequest("Email already exists.");
            //    }

            //    var adminEmails = new List<string> { "eiffa@gmail.com", "admin@example.com" };
            //    var role = adminEmails.Contains(registerModel.Email.ToLower()) ? "Admin" : "User";

            //    var newUser = new User
            //    {
            //        Username = registerModel.Username,
            //        Email = registerModel.Email,
            //        Password = registerModel.Password,
            //        Role = role
            //    };

            //    await _userService.RegisterAsync(newUser);

            //    var insertUser = _userService.GetUserByUsernameAndPassword(newUser.Username, newUser.Password);
            //    if (insertUser == null)
            //    {
            //        return BadRequest("User not found after registration.");
            //    }

            //    var token = JwtTokenHelper.GenerateToken(insertUser);
            //    return Ok(new
            //    {
            //        Token = token,
            //        UserId = insertUser.Id,
            //        Username = insertUser.Username,
            //        Email = insertUser.Email,
            //        Role = insertUser.Role // admin or user
            //    });
        }

      

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                if (login == null || login.Email == null)
                {
                    return BadRequest("Login details are missing.");
                }
                var user = await _userService.GetUserByEmailAsync(login.Email);
                if (user == null)
                    return Unauthorized("User  not found");
                if (string.IsNullOrEmpty(user.Password) || user.Password != login.Password)
                    return Unauthorized("Invalid password");
                var token = JwtTokenHelper.GenerateToken(user);
                return Ok(new
                {
                    Token = token,
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role // Ensure the role is included in the response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for {Email}", login.Email);
                return StatusCode(500, "Login failed - please try again");
            }
        }

            //user pai sahi login
            //try
            //{
            //    if (login == null || login.Email == null)
            //    {
            //        return BadRequest("Login details are missing.");
            //    }
            //    var user = await _userService.GetUserByEmailAsync(login.Email);
            //    if (user == null)
            //        return Unauthorized("User not found");

            //    if (string.IsNullOrEmpty(user.Password) || user.Password != login.Password)
            //        return Unauthorized("Invalid password");
            //    var token = JwtTokenHelper.GenerateToken(user);

            //    return Ok(new
            //    {
            //        Token = token,
            //        UserId = user.Id,
            //        Username = user.Username,
            //        Email = user.Email,
            //        Role = user.Role
            //    });
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Login failed for {Email}", login.Email);
            //    return StatusCode(500, "Login failed - please try again");
            //}

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Logged out");
        }

        //[HttpGet("{id}")]
        //public ActionResult<User> GetUserById(int id)
        //{
        //    var user = _userService.GetByIdAsync(id);
        //    if (user == null)
        //        return NotFound();

        //    return Ok(user);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                AllowedLeavesPerYear = user.AllowedLeavesPerYear,
                LeavesTakenThisYear = user.LeavesTakenThisYear,
                Year = user.Year
            };

            return Ok(userDto);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var user = await _userService.GetByIdAsync(int.Parse(userIdClaim.Value));
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.Role
            });
        }
    }
}