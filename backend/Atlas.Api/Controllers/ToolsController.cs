using Atlas.Application.Common.Models;
using Atlas.Application.Tools;
using Atlas.Application.Tools.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Api.Controllers;

[ApiController]
[Route("api/tools")]
public sealed class ToolsController(IToolService toolService) : ControllerBase
{
    /// <summary>Returns a paginated list of tools, optionally filtered by search term, category or license type.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ToolListDto>>> GetAll(
        [FromQuery] ToolQueryParameters query,
        CancellationToken cancellationToken)
    {
        var result = await toolService.GetPagedAsync(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns a single tool with its linked departments.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ToolDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var tool = await toolService.GetByIdAsync(id, cancellationToken);
        return Ok(tool);
    }

    /// <summary>Creates a new tool.</summary>
    [HttpPost]
    public async Task<ActionResult<ToolDetailDto>> Create(CreateToolDto dto, CancellationToken cancellationToken)
    {
        var created = await toolService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Replaces the editable fields of an existing tool.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ToolDetailDto>> Update(int id, UpdateToolDto dto, CancellationToken cancellationToken)
    {
        var updated = await toolService.UpdateAsync(id, dto, cancellationToken);
        return Ok(updated);
    }

    /// <summary>Deletes a tool.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await toolService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
