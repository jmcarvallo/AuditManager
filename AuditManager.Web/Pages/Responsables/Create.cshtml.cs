using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Responsables.Commands.Create;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Responsables;

public class CreateModel(IAuditoriaApiClient apiClient) : PageModel
{
    [BindProperty]
    public CreateResponsableCommand Command { get; set; } = new("", "", "");

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await apiClient.CreateResponsableAsync(Command);
            return RedirectToPage("/Responsables/Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error al crear el responsable: {ex.Message}");
            return Page();
        }
    }
}
