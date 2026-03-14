using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuditManager.Application.Features.Hallazgos.Commands.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Hallazgos;

public class CreateModel(HttpClient httpClient) : PageModel
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
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var response = await httpClient.PostAsJsonAsync("api/hallazgos", Command);
            if (!response.IsSuccessStatusCode)
            {
               var error = await response.Content.ReadAsStringAsync();
               ModelState.AddModelError(string.Empty, $"Error de API: {error}");
               return Page();
            }
            return RedirectToPage("/Auditorias/Details", new { id = Command.AuditoriaId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error al crear el hallazgo: {ex.Message}");
            return Page();
        }
    }
}
