using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Responsables.Commands.Create;
using AuditManager.Application.Features.Responsables.Queries.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuditManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResponsablesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await mediator.Send(new GetResponsablesListQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateResponsableCommand command)
    {
        var id = await mediator.Send(command);
        return Ok(id);
    }
}
