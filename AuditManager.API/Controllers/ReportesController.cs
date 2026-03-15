using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Reportes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Devuelve un resumen de auditorías finalizadas con hallazgos por severidad.
    /// Consume la vista SQL vw_AuditoriasFinalizadasResumen.
    /// Filtrables por rango de fecha de finalización.
    /// </summary>
    [HttpGet("auditorias-finalizadas")]
    public async Task<IActionResult> GetAuditoriasFinalizadas(
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin)
    {
        var result = await mediator.Send(new GetReporteAuditoriasQuery(fechaInicio, fechaFin));
        return Ok(result);
    }
}
