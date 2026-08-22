using System.Security.Claims;
using ActivityTracker.Application.DTOs;
using ActivityTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _service;

    public CompaniesController(ICompanyService service)
    {
        _service = service;
    }

    private string UserRole =>
        User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

    private int? CompanyId =>
        User.Claims.FirstOrDefault(c => c.Type == "company_id")?.Value is string v && int.TryParse(v, out var id)
            ? id : null;

    private bool IsGlobalAdmin => UserRole == "admin";

    /// <summary>Verifies the requester can access the given company (admin = all; company_admin = own only).</summary>
    private bool CanAccessCompany(int companyId) =>
        IsGlobalAdmin || (UserRole == "company_admin" && CompanyId == companyId);

    // ── Company CRUD ──────────────────────────────────────────────────────────

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!CanAccessCompany(id)) return Forbid();
        var company = await _service.GetByIdAsync(id);
        return company is null ? NotFound() : Ok(company);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var result = await _service.DeactivateAsync(id);
        return result ? NoContent() : NotFound();
    }

    // ── Company Users ─────────────────────────────────────────────────────────

    [HttpGet("{companyId:int}/users")]
    public async Task<IActionResult> GetUsers(int companyId)
    {
        if (!CanAccessCompany(companyId)) return Forbid();
        return Ok(await _service.GetUsersAsync(companyId));
    }

    [HttpPost("{companyId:int}/users")]
    public async Task<IActionResult> CreateUser(int companyId, [FromBody] CreateCompanyUserDto dto)
    {
        if (!CanAccessCompany(companyId)) return Forbid();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var created = await _service.CreateUserAsync(companyId, dto);
            return Ok(created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{companyId:int}/users/{userId:int}")]
    public async Task<IActionResult> DeactivateUser(int companyId, int userId)
    {
        if (!CanAccessCompany(companyId)) return Forbid();
        var result = await _service.DeactivateUserAsync(companyId, userId);
        return result ? NoContent() : NotFound();
    }
}
