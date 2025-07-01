using BlazorApp.Interface;
using Shared.Entities;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
namespace BlazorApp.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly HttpClient _http;

        public LeaveRequestService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<LeaveRequest>> GetUserLeaveRequestsAsync(int userId)
        {
            var response = await _http.GetAsync($"api/LeaveRequest/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<LeaveRequest>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Unable to fetch leaves for this user: {message}.");
            }
        }


        public async Task<ServiceResponse> CreateLeaveRequestAsync(LeaveRequestDto leave)
        {

            var response = await _http.PostAsJsonAsync($"api/LeaveRequest/create", leave);

            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("API response raw content: " + responseContent);

            if (response.IsSuccessStatusCode)
            {
                // Success
                return JsonSerializer.Deserialize<ServiceResponse>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                var error = JsonSerializer.Deserialize<ServiceResponse>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new ServiceResponse
                {
                    Success = false,
                    Message = error?.Message ?? "API responded with an error."
                };
            }


        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsByStatusAsync(string status)
        {
            return await _http.GetFromJsonAsync<List<LeaveRequest>>($"api/LeaveRequest/status/{status}");
        }

        public async Task UpdateLeaveStatusAsync(int id, string status, string remarks)
        {
            var update = new { Status = status, AdminRemarks = remarks };
            await _http.PostAsJsonAsync($"api/Admin/approve/{id}",remarks);
        }
        public async Task ApproveLeaveAsync(int id, string adminRemarks)
        {
            //var response = await _http.PostAsJsonAsync($"api/Admin/approve/{id}", new { adminRemarks });
            //if (!response.IsSuccessStatusCode)
            //{
            //    var message = await response.Content.ReadAsStringAsync();
            //    throw new Exception($"Approval failed: {message}.");
            //}
            var response = await _http.PostAsJsonAsync($"api/Admin/approve/{id}",
      new { adminRemarks });

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Approval failed: {message}.");
            }
        }
        public async Task RejectLeaveAsync(int leaveId, string adminRemarks)
        {
            //var response = await _http.PostAsJsonAsync($"api/Admin/reject/{leaveId}", adminRemarks);
            //response.EnsureSuccessStatusCode();
            await _http.PostAsJsonAsync($"api/Admin/reject/{leaveId}",
            new { AdminRemarks = adminRemarks });


        }

        public async Task<List<LeaveRequestDto>> LoadPendingLeavesAsync()
        {
            var response = await _http.GetAsync("api/admin/pending");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<LeaveRequestDto>>();
        }

        public async Task<List<LeaveRequestDto>> LoadApprovedLeavesAsync()
        {
            var response = await _http.GetAsync("api/admin/approvedrejected");
            response.EnsureSuccessStatusCode();
            var allApprovedRejected = await response.Content.ReadFromJsonAsync<List<LeaveRequestDto>>();

            return allApprovedRejected.Where(l => l.Status == "Approved").ToList();
        }
        public async Task<List<LeaveRequestDto>> LoadLeavesAsync()
        {
            var response = await _http.GetAsync("api/admin/all");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<LeaveRequestDto>>();
        }
        public async Task<List<LeaveRequestDto>> GetAllLeaveRequestsAsync()
        {
            var response = await _http.GetAsync("api/admin/all"); 

            if (response.IsSuccessStatusCode)
            {
                var leaveRequests = await response.Content.ReadFromJsonAsync<List<LeaveRequestDto>>();
                return leaveRequests ?? new List<LeaveRequestDto>();
            }
            else
            {
                return new List<LeaveRequestDto>();
            }
        }

    }
}
