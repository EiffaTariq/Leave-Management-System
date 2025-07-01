//using Shared.Entities;

//namespace BlazorApp.Interface
//{
//    public interface IAuthService
//    {
//        Task<AuthResult> LoginAsync(LoginRequest request);
//        Task LogoutAsync();
//        Task<bool> IsAuthenticatedAsync();
//    }

//    public class AuthResult
//    {
//        public bool Successful { get; init; }
//        public string Error { get; init; } = string.Empty;

//        public static AuthResult Success() => new() { Successful = true };
//        public static AuthResult Failure(string error) => new() { Successful = false, Error = error };
//    }
//}
