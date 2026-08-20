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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ActivityQueryParams queryParams) =>
        Ok(await _service.GetAllAsync(queryParams));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var activity = await _service.GetByIdAsync(id);
        return activity is null ? NotFound() : Ok(activity);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateActivityDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
