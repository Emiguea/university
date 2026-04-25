using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionsController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Permission>>> GetAllPermissions()
    {
        var permissions = await _permissionService.GetAllPermissionsAsync();
        return Ok(permissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Permission>> GetPermissionById(int id)
    {
        var permission = await _permissionService.GetPermissionByIdAsync(id);
        if (permission == null)
            return NotFound();

        return Ok(permission);
    }

    [HttpGet("bycode/{permissionCode}")]
    public async Task<ActionResult<Permission>> GetPermissionByCode(string permissionCode)
    {
        var permission = await _permissionService.GetPermissionByCodeAsync(permissionCode);
        if (permission == null)
            return NotFound();

        return Ok(permission);
    }

    [HttpGet("bymodule/{module}")]
    public async Task<ActionResult<IEnumerable<Permission>>> GetPermissionsByModule(string module)
    {
        var permissions = await _permissionService.GetPermissionsByModuleAsync(module);
        return Ok(permissions);
    }

    [HttpGet("parents")]
    public async Task<ActionResult<IEnumerable<Permission>>> GetParentPermissions()
    {
        var permissions = await _permissionService.GetParentPermissionsAsync();
        return Ok(permissions);
    }

    [HttpGet("{parentId}/children")]
    public async Task<ActionResult<IEnumerable<Permission>>> GetChildPermissions(int parentId)
    {
        var permissions = await _permissionService.GetChildPermissionsAsync(parentId);
        return Ok(permissions);
    }

    [HttpPost]
    public async Task<ActionResult<Permission>> CreatePermission(Permission permission)
    {
        try
        {
            var createdPermission = await _permissionService.CreatePermissionAsync(permission);
            return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.Id }, createdPermission);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(int id, Permission permission)
    {
        if (id != permission.Id)
            return BadRequest();

        var existingPermission = await _permissionService.GetPermissionByIdAsync(id);
        if (existingPermission == null)
            return NotFound();

        await _permissionService.UpdatePermissionAsync(permission);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(int id)
    {
        try
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null)
                return NotFound();

            await _permissionService.DeletePermissionAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}