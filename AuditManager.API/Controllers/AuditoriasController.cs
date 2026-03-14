using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Auditorias.Commands.Create;
using AuditManager.Application.Features.Auditorias.Commands.Update;
using AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;
using AuditManager.Application.Features.Auditorias.Queries.GetAuditoriasList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuditManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriasController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAuditoriasListQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuditoriaCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAuditoriaCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{id}/estado")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] UpdateAuditoriaStatusCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID de la ruta no coincide con el del cuerpo.");

        try
        {
            var result = await mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
