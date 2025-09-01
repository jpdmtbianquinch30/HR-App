using backend.DTOs;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(string username, string password);
        Task<bool> Register(User user, string password);
        Task<bool> ChangePassword(int userId, string currentPassword, string newPassword);
    }
}