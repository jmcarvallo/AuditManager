using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auditorias.Commands.Create;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditManager.Web.Pages.Auditorias;

public class CreateModel(IAuditoriaApiClient apiClient) : PageModel
{
    [BindProperty]
    public CreateAuditoriaCommand Command { get; set; } = new CreateAuditoriaCommand("", DateTime.Now, DateTime.Now.AddDays(1), "", Guid.Empty);

    public List<SelectListItem> ResponsablesSelectList { get; set; } = [];

    public async Task OnGetAsync()
    {
        await LoadResponsables();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadResponsables();
            return Page();
        }

        try
        {
            await apiClient.CreateAuditoriaAsync(Command);
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error al crear la auditoría: {ex.Message}");
            await LoadResponsables();
            return Page();
        }
    }

    private async Task LoadResponsables()
    {
        var lista = await apiClient.GetResponsablesAsync();
        ResponsablesSelectList = lista.ConvertAll(r => new SelectListItem(
            $"{r.Nombre} ({r.Area})", r.Id.ToString()));
    }
}
