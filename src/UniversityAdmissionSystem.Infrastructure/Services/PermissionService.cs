using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class PermissionService : IPermissionService
{
    private readonly IRepository<Permission> _permissionRepository;
    private readonly ApplicationDbContext _context;

    public PermissionService(
        IRepository<Permission> permissionRepository,
        ApplicationDbContext context)
    {
        _permissionRepository = permissionRepository;
        _context = context;
    }

    public async Task<Permission?> GetPermissionByIdAsync(int id)
    {
        return await _context.Permissions
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Permission?> GetPermissionByCodeAsync(string permissionCode)
    {
        return await _context.Permissions
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .FirstOrDefaultAsync(p => p.PermissionCode == permissionCode);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permissions
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetPermissionsByModuleAsync(string module)
    {
        return await _context.Permissions
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .Where(p => p.Module == module)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetParentPermissionsAsync()
    {
        return await _context.Permissions
            .Include(p => p.Children)
            .Where(p => p.ParentId == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<Permission>> GetChildPermissionsAsync(int parentId)
    {
        return await _context.Permissions
            .Include(p => p.Parent)
            .Where(p => p.ParentId == parentId)
            .ToListAsync();
    }

    public async Task<Permission> CreatePermissionAsync(Permission permission)
    {
        if (!string.IsNullOrEmpty(permission.PermissionCode))
        {
            var existingPermission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.PermissionCode == permission.PermissionCode);

            if (existingPermission != null)
                throw new InvalidOperationException("Permission code already exists");
        }

        permission.CreatedAt = DateTime.Now;
        await _permissionRepository.AddAsync(permission);
        await _permissionRepository.SaveChangesAsync();
        return permission;
    }

    public async Task UpdatePermissionAsync(Permission permission)
    {
        var existingPermission = await _permissionRepository.GetByIdAsync(permission.Id);
        if (existingPermission == null)
            throw new KeyNotFoundException("Permission not found");

        existingPermission.PermissionName = permission.PermissionName;
        existingPermission.PermissionCode = permission.PermissionCode;
        existingPermission.Description = permission.Description;
        existingPermission.Module = permission.Module;
        existingPermission.ParentId = permission.ParentId;
        existingPermission.UpdatedAt = DateTime.Now;

        _permissionRepository.Update(existingPermission);
        await _permissionRepository.SaveChangesAsync();
    }

    public async Task DeletePermissionAsync(int id)
    {
        var permission = await _permissionRepository.GetByIdAsync(id);
        if (permission == null)
            return;

        var hasChildren = await _context.Permissions
            .AnyAsync(p => p.ParentId == id);

        if (hasChildren)
            throw new InvalidOperationException("Cannot delete permission with child permissions");

        var rolePermissions = await _context.RolePermissions
            .Where(rp => rp.PermissionId == id)
            .ToListAsync();

        foreach (var rp in rolePermissions)
        {
            _context.RolePermissions.Remove(rp);
        }

        _permissionRepository.Delete(permission);
        await _permissionRepository.SaveChangesAsync();
    }
}