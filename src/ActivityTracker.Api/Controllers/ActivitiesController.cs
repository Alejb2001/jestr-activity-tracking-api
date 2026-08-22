using System.Security.Claims;
using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Interfaces;
using ActivityTracker.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _service;

    public ActivitiesController(IActivityService service)
    {
        _service = service;
    }

    private int? TenantId =>
        User.Claims.FirstOrDefault(c => c.Type == "company_id")?.Value is string v && int.TryParse(v, out var id)
            ? id : null;

    private bool IsViewer =>
        User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value is "viewer" or "company_viewer";

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ActivityQueryParams queryParams) =>
        Ok(await _service.GetAllAsync(queryParams, TenantId));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var activity = await _service.GetByIdAsync(id);
        return activity is null ? NotFound() : Ok(activity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityDto dto)
    {
        if (IsViewer) return Forbid();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Override CompanyId with the tenant from the JWT (security)
        var dtoWithTenant = dto with { CompanyId = TenantId };
        var created = await _service.CreateAsync(dtoWithTenant);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateActivityDto dto)
    {
        if (IsViewer) return Forbid();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (IsViewer) return Forbid();
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
