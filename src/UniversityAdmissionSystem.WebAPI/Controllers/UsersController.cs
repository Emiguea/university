using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Security;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users.Select(u => new
        {
            u.Id,
            u.Username,
            u.FullName,
            u.Gender,
            PhoneNumber = DataMaskingHelper.MaskPhoneNumber(u.PhoneNumber),
            Email = DataMaskingHelper.MaskEmail(u.Email),
            u.Department,
            u.Position,
            u.IsActive,
            u.RoleId,
            u.LastLoginTime,
            u.CreatedAt,
            u.UpdatedAt
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.Username,
            user.FullName,
            user.Gender,
            PhoneNumber = DataMaskingHelper.MaskPhoneNumber(user.PhoneNumber),
            Email = DataMaskingHelper.MaskEmail(user.Email),
            user.Department,
            user.Position,
            user.IsActive,
            user.RoleId,
            user.LastLoginTime,
            user.CreatedAt,
            user.UpdatedAt
        });
    }

    [HttpGet("byusername/{username}")]
    public async Task<ActionResult<object>> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);
        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.Username,
            user.FullName,
            user.Gender,
            PhoneNumber = DataMaskingHelper.MaskPhoneNumber(user.PhoneNumber),
            Email = DataMaskingHelper.MaskEmail(user.Email),
            user.Department,
            user.Position,
            user.IsActive,
            user.RoleId,
            user.LastLoginTime,
            user.CreatedAt,
            user.UpdatedAt
        });
    }

    [HttpGet("byrole/{roleId}")]
    public async Task<ActionResult<IEnumerable<object>>> GetUsersByRole(int roleId)
    {
        var users = await _userService.GetUsersByRoleIdAsync(roleId);
        return Ok(users.Select(u => new
        {
            u.Id,
            u.Username,
            u.FullName,
            u.Gender,
            PhoneNumber = DataMaskingHelper.MaskPhoneNumber(u.PhoneNumber),
            Email = DataMaskingHelper.MaskEmail(u.Email),
            u.Department,
            u.Position,
            u.IsActive,
            u.RoleId,
            u.LastLoginTime,
            u.CreatedAt,
            u.UpdatedAt
        }));
    }

    [HttpPost]
    public async Task<ActionResult<object>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var passwordValidation = _userService.ValidatePasswordStrength(request.Password);
            if (!passwordValidation.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "密码不符合要求",
                    Errors = passwordValidation.Errors
                });
            }

            var user = new User
            {
                Username = request.Username,
                FullName = request.FullName,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Department = request.Department,
                Position = request.Position,
                RoleId = request.RoleId
            };

            var createdUser = await _userService.CreateUserAsync(user, request.Password);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, new
            {
                createdUser.Id,
                createdUser.Username,
                createdUser.FullName,
                createdUser.Gender,
                PhoneNumber = DataMaskingHelper.MaskPhoneNumber(createdUser.PhoneNumber),
                Email = DataMaskingHelper.MaskEmail(createdUser.Email),
                createdUser.Department,
                createdUser.Position,
                createdUser.IsActive,
                createdUser.RoleId,
                createdUser.CreatedAt
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null)
            return NotFound();

        var user = new User
        {
            Id = id,
            FullName = request.FullName,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Department = request.Department,
            Position = request.Position,
            RoleId = request.RoleId
        };

        await _userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();

        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var isValid = await _userService.ValidateUserCredentialsAsync(request.Username, request.Password);
        if (!isValid)
            return Unauthorized(new { Message = "用户名或密码错误" });

        var user = await _userService.GetUserByUsernameAsync(request.Username);
        if (user == null)
            return NotFound();

        if (!user.IsActive)
            return Unauthorized(new { Message = "用户已被禁用" });

        await _userService.UpdateLastLoginTimeAsync(user.Id);

        return Ok(new
        {
            user.Id,
            user.Username,
            user.FullName,
            user.RoleId,
            user.Department
        });
    }

    [HttpPost("{userId}/changepassword")]
    public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            var passwordValidation = _userService.ValidatePasswordStrength(request.NewPassword);
            if (!passwordValidation.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "新密码不符合要求",
                    Errors = passwordValidation.Errors
                });
            }

            await _userService.ChangePasswordAsync(userId, request.OldPassword, request.NewPassword);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{userId}/resetpassword")]
    public async Task<IActionResult> ResetPassword(int userId, [FromBody] ResetPasswordRequest request)
    {
        try
        {
            var passwordValidation = _userService.ValidatePasswordStrength(request.NewPassword);
            if (!passwordValidation.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "新密码不符合要求",
                    Errors = passwordValidation.Errors
                });
            }

            await _userService.ResetPasswordAsync(userId, request.NewPassword);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{userId}/activate")]
    public async Task<IActionResult> ActivateUser(int userId)
    {
        try
        {
            await _userService.ActivateUserAsync(userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{userId}/deactivate")]
    public async Task<IActionResult> DeactivateUser(int userId)
    {
        try
        {
            await _userService.DeactivateUserAsync(userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("password-requirements")]
    public ActionResult<object> GetPasswordRequirements()
    {
        return Ok(new
        {
            Requirements = _userService.GetPasswordRequirements(),
            MinimumLength = PasswordValidator.MinimumLength,
            MaximumLength = PasswordValidator.MaximumLength,
            RequireUppercase = PasswordValidator.RequireUppercase,
            RequireLowercase = PasswordValidator.RequireLowercase,
            RequireDigit = PasswordValidator.RequireDigit,
            RequireNonAlphanumeric = PasswordValidator.RequireNonAlphanumeric
        });
    }

    [HttpPost("validate-password")]
    public ActionResult<object> ValidatePassword([FromBody] string password)
    {
        var result = _userService.ValidatePasswordStrength(password);
        return Ok(new
        {
            IsValid = result.IsValid,
            Errors = result.Errors
        });
    }
}

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public int? RoleId { get; set; }
}

public class UpdateUserRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public int? RoleId { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}
