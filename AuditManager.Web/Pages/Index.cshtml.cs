using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Core.Enums;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditManager.Web.Pages;

public class IndexModel(IAuditoriaApiClient apiClient) : PageModel
{
    public List<AuditoriaDto> Auditorias { get; set; } = [];
    public List<SelectListItem> ResponsablesSelectList { get; set; } = [];
    public List<SelectListItem> EstadosSelectList { get; set; } =
    [
        new("Pendiente",   "0"),
        new("En Proceso",  "1"),
        new("Finalizada",  "2"),
    ];

    [BindProperty(SupportsGet = true)] public Guid?              ResponsableId { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime?          FechaInicio   { get; set; }
    [BindProperty(SupportsGet = true)] public DateTime?          FechaFin      { get; set; }
    [BindProperty(SupportsGet = true)] public EstadoAuditoria?   Estado        { get; set; }

    public async Task OnGetAsync()
    {
        var responsables = await apiClient.GetResponsablesAsync();
        ResponsablesSelectList = responsables.ConvertAll(r =>
            new SelectListItem($"{r.Nombre} ({r.Area})", r.Id.ToString()));

        Auditorias = await apiClient.GetAuditoriasAsync(
            responsableId: ResponsableId,
            fechaInicio:   FechaInicio,
            fechaFin:      FechaFin,
            estado:        Estado);
    }
}
