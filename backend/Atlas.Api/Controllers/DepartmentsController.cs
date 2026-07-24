using Atlas.Application.Departments;
using Atlas.Application.Departments.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Api.Controllers;

[ApiController]
[Route("api/departments")]
public sealed class DepartmentsController(IDepartmentService departmentService) : ControllerBase
{
    /// <summary>Returns every department.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<DepartmentListDto>>> GetAll(CancellationToken cancellationToken)
    {
        var departments = await departmentService.GetAllAsync(cancellationToken);
        return Ok(departments);
    }

    /// <summary>Returns a single department with its linked tools.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartmentDetailDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var department = await departmentService.GetByIdAsync(id, cancellationToken);
        return Ok(department);
    }

    /// <summary>Returns the tools linked to a department.</summary>
    [HttpGet("{id:int}/tools")]
    public async Task<ActionResult<IReadOnlyCollection<DepartmentToolLinkDto>>> GetTools(int id, CancellationToken cancellationToken)
    {
        var tools = await departmentService.GetToolsAsync(id, cancellationToken);
        return Ok(tools);
    }

    /// <summary>Links an existing tool to a department.</summary>
    [HttpPost("{id:int}/tools")]
    public async Task<ActionResult<DepartmentToolLinkDto>> LinkTool(int id, LinkToolDto dto, CancellationToken cancellationToken)
    {
        var link = await departmentService.LinkToolAsync(id, dto, cancellationToken);
        return CreatedAtAction(nameof(GetTools), new { id }, link);
    }

    /// <summary>Removes the link between a department and a tool.</summary>
    [HttpDelete("{id:int}/tools/{toolId:int}")]
    public async Task<IActionResult> UnlinkTool(int id, int toolId, CancellationToken cancellationToken)
    {
        await departmentService.UnlinkToolAsync(id, toolId, cancellationToken);
        return NoContent();
    }
}
