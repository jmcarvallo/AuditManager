using System;
using System.Threading.Tasks;
using AuditManager.Application.Features.Auditorias.Commands.Create;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Auditorias;

public class CreateModel(IAuditoriaApiClient apiClient) : PageModel
{
    [BindProperty]
    public CreateAuditoriaCommand Command { get; set; } = new CreateAuditoriaCommand("", DateTime.Now, DateTime.Now.AddDays(1), "", Guid.Empty);

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
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
            return Page();
        }
    }
}
