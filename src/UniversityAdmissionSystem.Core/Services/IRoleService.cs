using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IRoleService
{
    Task<Role?> GetRoleByIdAsync(int id);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role> CreateRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
    Task DeleteRoleAsync(int id);
    Task AssignPermissionAsync(int roleId, int permissionId);
    Task RemovePermissionAsync(int roleId, int permissionId);
    Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId);
    Task<bool> RoleHasPermissionAsync(int roleId, string permissionCode);
}