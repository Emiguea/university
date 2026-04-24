namespace UniversityAdmissionSystem.Core.Models;

public class Permission
{
    public int Id { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public string? PermissionCode { get; set; }
    public string? Description { get; set; }
    public string? Module { get; set; }
    public int? ParentId { get; set; }
    public Permission? Parent { get; set; }
    public ICollection<Permission>? Children { get; set; }
    public ICollection<RolePermission>? RolePermissions { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}