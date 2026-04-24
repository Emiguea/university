namespace UniversityAdmissionSystem.Core.Models;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }
    public ICollection<User>? Users { get; set; }
    public ICollection<RolePermission>? RolePermissions { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}