using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId);
    Task<User> CreateUserAsync(User user, string password);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    Task ResetPasswordAsync(int userId, string newPassword);
    Task ActivateUserAsync(int userId);
    Task DeactivateUserAsync(int userId);
    Task UpdateLastLoginTimeAsync(int userId);
}