using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Hallazgos.Commands.Create;
using AuditManager.Application.Features.Hallazgos.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuditManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HallazgosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHallazgoCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return StatusCode(201, new { id = result }); // We can't use CreatedAtRoute because Get isn't implemented yet, but keeping pragmatic
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await mediator.Send(new DeleteHallazgoCommand(id));
            if (!result) return NotFound();
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
