using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using WebApi.Interface;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<User>> GetAllAsync() => _userRepository.GetAllAsync();

        public Task<User?> GetByIdAsync(int id) => _userRepository.GetByIdAsync(id);
        public Task<User?> GetUserByEmailAsync(string email) => _userRepository.GetUserByEmailAsync(email);
        public User? GetUserByEmailAndPassword(string email, string password)
        {
            return _userRepository.GetUserByEmailAndPassword(email, password);
        }
        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            return _userRepository.GetUserByEmailAndPassword(username, password);
        }
        public List<UserLeaveBalance> GetUserLeaveBalances(int userId)
        {
            return _userRepository.GetLeaveBalancesByUserId(userId);
        }



        //public async Task<ServiceResponse> RegisterUserAsync(RegisterDto dto)
        //{
        //    if (dto.password != dto.confirmPassword)
        //        return new ServiceResponse { Success = false, Message = "Passwords do not match." };

        //    var existingUser = await _userRepository.GetByUsernameAsync(dto.name);
        //    if (existingUser != null)
        //        return new ServiceResponse { Success = false, Message = "Username already exists." };

        //    var user = new User
        //    {
        //        Username = dto.name,
        //        Password = dto.password, 
        //        Role = dto.role
        //    };

        //    await _userRepository.AddAsync(user);
        //    return new ServiceResponse { Success = true, Message = "Registration successful" };
        //}

        //public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        //{
        //    var user = new User
        //    {
        //        Username = dto.name,
        //        Password = dto.password,
        //        Role = dto.role
        //    };

        //    await _userRepository.AddAsync(user);

        //    return new ServiceResponse
        //    {
        //        Success = true,
        //        Message = "Registration successful"
        //    };
        //}


        //public async Task<ServiceResponse> RegisterAsync(RegisterDto dto)
        //{
        //    if (dto == null)
        //        return new ServiceResponse { Success = false, Message = "Invalid data" };

        //    var existingUser = _userRepository.GetByUsernameAsync(dto.name);
        //    if (existingUser != null)
        //    {
        //        return new ServiceResponse { Success = false, Message = "Username already exists." };
        //    }

        //    var user = new User
        //    {
        //        Username = dto.name,
        //        Password = dto.password,
        //        Role = dto.role
        //    };

        //    _userRepository.AddAsync(user);

        //    return new ServiceResponse { Success = true, Message = "Registration successful" };
        //}


        public async Task RegisterAsync(User user)
        {
            
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email is required");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required");

            user.Role ??= "User";
            await _userRepository.AddAsync(user);
        }


        public Task AddAsync(User user) => _userRepository.AddAsync(user);

        public Task UpdateAsync(User user) => _userRepository.UpdateAsync(user);

        public Task DeleteAsync(int id) => _userRepository.DeleteAsync(id);
    }

}
