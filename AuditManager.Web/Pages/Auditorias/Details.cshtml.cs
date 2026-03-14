using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Auditorias;

public class DetailsModel(IAuditoriaApiClient apiClient) : PageModel
{
    public AuditoriaDto Auditoria { get; set; } = default!;
    public List<HallazgoDto> Hallazgos { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var auditoria = await apiClient.GetAuditoriaByIdAsync(id);
        if (auditoria == null) return NotFound();

        Auditoria = auditoria;
        Hallazgos = await apiClient.GetHallazgosByAuditoriaAsync(id);
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
