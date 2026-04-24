using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly ApplicationDbContext _context;

    public UserService(
        IRepository<User> userRepository,
        ApplicationDbContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r != null ? r.RolePermissions : null)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r != null ? r.RolePermissions : null)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == user.Username);

        if (existingUser != null)
            throw new InvalidOperationException("Username already exists");

        user.PasswordHash = HashPassword(password);
        user.IsActive = true;
        user.CreatedAt = DateTime.Now;

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id);
        if (existingUser == null)
            throw new KeyNotFoundException("User not found");

        existingUser.FullName = user.FullName;
        existingUser.Gender = user.Gender;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Email = user.Email;
        existingUser.Department = user.Department;
        existingUser.Position = user.Position;
        existingUser.RoleId = user.RoleId;
        existingUser.UpdatedAt = DateTime.Now;

        _userRepository.Update(existingUser);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user != null)
        {
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
        }
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

        if (user == null)
            return false;

        return VerifyPassword(password, user.PasswordHash);
    }

    public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (!VerifyPassword(oldPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid old password");

        user.PasswordHash = HashPassword(newPassword);
        user.UpdatedAt = DateTime.Now;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task ResetPasswordAsync(int userId, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.PasswordHash = HashPassword(newPassword);
        user.UpdatedAt = DateTime.Now;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task ActivateUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.IsActive = true;
        user.UpdatedAt = DateTime.Now;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeactivateUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.IsActive = false;
        user.UpdatedAt = DateTime.Now;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task UpdateLastLoginTimeAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            user.LastLoginTime = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        return hash == storedHash;
    }
}