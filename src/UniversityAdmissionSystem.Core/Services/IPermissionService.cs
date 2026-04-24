using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IPermissionService
{
    Task<Permission?> GetPermissionByIdAsync(int id);
    Task<Permission?> GetPermissionByCodeAsync(string permissionCode);
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    Task<IEnumerable<Permission>> GetPermissionsByModuleAsync(string module);
    Task<IEnumerable<Permission>> GetParentPermissionsAsync();
    Task<IEnumerable<Permission>> GetChildPermissionsAsync(int parentId);
    Task<Permission> CreatePermissionAsync(Permission permission);
    Task UpdatePermissionAsync(Permission permission);
    Task DeletePermissionAsync(int id);
}