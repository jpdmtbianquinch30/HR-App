using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Authentifie un utilisateur avec son nom d'utilisateur et son mot de passe.
        /// Retourne un LoginResponse si succès, null sinon.
        /// </summary>
        Task<LoginResponse?> Login(string username, string password);

        /// <summary>
        /// Enregistre un nouvel utilisateur avec un mot de passe.
        /// Retourne true si succès, false sinon.
        /// </summary>
        Task<bool> Register(User user, string password);

        /// <summary>
        /// Change le mot de passe d'un utilisateur.
        /// Retourne true si succès, false sinon.
        /// </summary>
        Task<bool> ChangePassword(int userId, string currentPassword, string newPassword);
    }
}
