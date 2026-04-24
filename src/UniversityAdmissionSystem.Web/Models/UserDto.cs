namespace UniversityAdmissionSystem.Web.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public bool IsActive { get; set; }
    public int? RoleId { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}