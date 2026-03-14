using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Reportes;

public class IndexModel(IAuditoriaApiClient apiClient) : PageModel
{
    public List<AuditoriaResumenDto> Reporte { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public DateTime? FechaInicio { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FechaFin { get; set; }

    public async Task OnGetAsync()
    {
        Reporte = await apiClient.GetReporteAsync(FechaInicio, FechaFin);
    }
}
