using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Auditorias.Commands.Update;
using AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Auditorias;

public class EditModel(IAuditoriaApiClient apiClient) : PageModel
{
    [BindProperty]
    public UpdateAuditoriaCommand Command { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var auditoria = await apiClient.GetAuditoriaByIdAsync(id);
        if (auditoria == null)
        {
            return NotFound();
        }

        if (auditoria.Estado != Core.Enums.EstadoAuditoria.Pendiente)
        {
            TempData["Error"] = "Solo se puede editar una auditoría en estado Pendiente.";
            return RedirectToPage("/Index");
        }

        Command = new UpdateAuditoriaCommand(
            auditoria.Id, 
            auditoria.Titulo, 
            auditoria.FechaInicio, 
            auditoria.FechaFin, 
            auditoria.AreaAuditada, 
            auditoria.ResponsableId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var success = await apiClient.UpdateAuditoriaAsync(Command.Id, Command);
            if (!success)
            {
               ModelState.AddModelError(string.Empty, "Actualización fallida.");
               return Page();
            }
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error al actualizar la auditoría: {ex.Message}");
            return Page();
        }
    }

    public async Task<IActionResult> OnPostStartAuditAsync()
    {
        try
        {
            var commandStatus = new UpdateAuditoriaStatusCommand(Command.Id, Core.Enums.EstadoAuditoria.EnProceso);
            await apiClient.ChangeStatusAsync(Command.Id, commandStatus);
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error al iniciar la auditoría: {ex.Message}");
            return Page();
        }
    }
}
