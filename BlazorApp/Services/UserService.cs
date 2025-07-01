using Shared.Entities;
using System.Net.Http.Json;
using BlazorApp.Interface;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using Blazored.SessionStorage;
namespace BlazorApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;
        //private readonly string _baseUrl = "https://localhost:44391/api/User";
        private readonly ISessionStorageService _sessionStorage;

        public UserService(HttpClient http, ISessionStorageService sessionStorage)
        {
            _http = http;
            _sessionStorage = sessionStorage;
        }

        public async Task<List<LeaveRequest>?> GetUserLeaveRequestsAsync(int userId)
        {
            return await _http.GetFromJsonAsync<List<LeaveRequest>>($"api/LeaveRequest/user/{userId}");
        }


        //public async Task<User?> GetUserByIdAsync(int id)
        //{
        //    //return await _http.GetFromJsonAsync<User>($"api/User/{id}");
        //    return await _http.GetFromJsonAsync<User>($"{_baseUrl}/{id}");
        //}
        public async Task<User> GetUserByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/user/{id}");

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<User>())!;
        }

        public async Task<List<UserLeaveBalance>?> GetUserLeaveBalancesAsync(int userId)
        {
            return await _http.GetFromJsonAsync<List<UserLeaveBalance>>($"api/User/{userId}/leave-balances");
        }

        //public async Task<bool> RegisterUserAsync(RegisterDto model)
        //{
        //    var response = await _http.PostAsJsonAsync("api/User/register", model);
        //    return response.IsSuccessStatusCode;
        //}
        public async Task<string> RegisterUserAsync(RegisterDto model)
        {
            var response = await _http.PostAsJsonAsync("api/User/register", model);

            if (response.IsSuccessStatusCode)
            {
                return "Registration successful";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return $"Registration failed: {error}";
            }
        }
        public async Task<bool> LoginAsync(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/User/login", dto);
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<UserLoginResult>();
            if (result == null)
            {
                return false;
            }
            await _sessionStorage.SetItemAsync("UserId", result.UserId);
            await _sessionStorage.SetItemAsync("Username", result.Username);
            await _sessionStorage.SetItemAsync("UserRole", result.Role);
            await _sessionStorage.SetItemAsync("email", result.Email);
            return true;
        }

    }

}
