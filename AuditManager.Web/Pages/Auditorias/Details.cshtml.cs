using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;
using AuditManager.Core.Enums;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditManager.Web.Pages.Auditorias;

public class DetailsModel(IAuditoriaApiClient apiClient) : PageModel
{
    public AuditoriaDto Auditoria { get; set; } = default!;
    public List<HallazgoDto> Hallazgos { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public SeveridadHallazgo? SeveridadFiltro { get; set; }

    public List<SelectListItem> SeveridadesSelectList { get; } =
    [
        new("Baja",  "0"),
        new("Media", "1"),
        new("Alta",  "2"),
    ];

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var auditoria = await apiClient.GetAuditoriaByIdAsync(id);
        if (auditoria == null) return NotFound();

        Auditoria = auditoria;
        var todos = await apiClient.GetHallazgosByAuditoriaAsync(id);
        Hallazgos = SeveridadFiltro.HasValue
            ? todos.Where(h => h.Severidad == SeveridadFiltro.Value).ToList()
            : todos;
        return Page();
    }

    public async Task<IActionResult> OnPostFinalizeAsync(Guid id)
    {
        try
        {
            var commandStatus = new UpdateAuditoriaStatusCommand(id, Core.Enums.EstadoAuditoria.Finalizada);
            await apiClient.ChangeStatusAsync(id, commandStatus);
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error al finalizar: {ex.Message}";
            return RedirectToPage(new { id });
        }
    }

    public async Task<IActionResult> OnPostDeleteHallazgoAsync(Guid hallazgoId, Guid auditoriaId)
    {
        try
        {
            await apiClient.DeleteHallazgoAsync(hallazgoId);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error al eliminar hallazgo: {ex.Message}";
        }
        return RedirectToPage(new { id = auditoriaId });
    }
}
