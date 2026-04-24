using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _roleRepository;
    private readonly IRepository<Permission> _permissionRepository;
    private readonly IRepository<RolePermission> _rolePermissionRepository;
    private readonly ApplicationDbContext _context;

    public RoleService(
        IRepository<Role> roleRepository,
        IRepository<Permission> permissionRepository,
        IRepository<RolePermission> rolePermissionRepository,
        ApplicationDbContext context)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _context = context;
    }

    public async Task<Role?> GetRoleByIdAsync(int id)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.RoleName == roleName);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        var existingRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == role.RoleName);

        if (existingRole != null)
            throw new InvalidOperationException("Role name already exists");

        role.CreatedAt = DateTime.Now;
        await _roleRepository.AddAsync(role);
        await _roleRepository.SaveChangesAsync();
        return role;
    }

    public async Task UpdateRoleAsync(Role role)
    {
        var existingRole = await _roleRepository.GetByIdAsync(role.Id);
        if (existingRole == null)
            throw new KeyNotFoundException("Role not found");

        if (existingRole.IsSystemRole && existingRole.RoleName != role.RoleName)
            throw new InvalidOperationException("Cannot rename system role");

        existingRole.RoleName = role.RoleName;
        existingRole.Description = role.Description;
        existingRole.UpdatedAt = DateTime.Now;

        _roleRepository.Update(existingRole);
        await _roleRepository.SaveChangesAsync();
    }

    public async Task DeleteRoleAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null)
            return;

        if (role.IsSystemRole)
            throw new InvalidOperationException("Cannot delete system role");

        var usersInRole = await _context.Users
            .AnyAsync(u => u.RoleId == id);

        if (usersInRole)
            throw new InvalidOperationException("Cannot delete role with users assigned");

        var rolePermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == id)
            .ToListAsync();

        foreach (var rp in rolePermissions)
        {
            _rolePermissionRepository.Delete(rp);
        }

        _roleRepository.Delete(role);
        await _roleRepository.SaveChangesAsync();
    }

    public async Task AssignPermissionAsync(int roleId, int permissionId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        var permission = await _permissionRepository.GetByIdAsync(permissionId);

        if (role == null || permission == null)
            throw new KeyNotFoundException("Role or permission not found");

        var existingRolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (existingRolePermission != null)
            return;

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        await _rolePermissionRepository.AddAsync(rolePermission);
        await _rolePermissionRepository.SaveChangesAsync();
    }

    public async Task RemovePermissionAsync(int roleId, int permissionId)
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (rolePermission != null)
        {
            _rolePermissionRepository.Delete(rolePermission);
            await _rolePermissionRepository.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.Permission!)
            .ToListAsync();
    }

    public async Task<bool> RoleHasPermissionAsync(int roleId, string permissionCode)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .AnyAsync(rp => rp.RoleId == roleId && rp.Permission!.PermissionCode == permissionCode);
    }
}