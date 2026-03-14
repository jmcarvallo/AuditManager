using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Hallazgos.Commands.Create;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Hallazgos;

public class CreateModel(IAuditoriaApiClient apiClient) : PageModel
{
    [BindProperty]
    public CreateHallazgoCommand Command { get; set; } = default!;

    public IActionResult OnGet(Guid auditoriaId)
    {
        Command = new CreateHallazgoCommand(auditoriaId, "", Core.Enums.TipoHallazgo.Observacion, Core.Enums.SeveridadHallazgo.Baja, DateTime.Now);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await apiClient.CreateHallazgoAsync(Command);
            return RedirectToPage("/Auditorias/Details", new { id = Command.AuditoriaId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error al crear el hallazgo: {ex.Message}");
            return Page();
        }
    }
}
