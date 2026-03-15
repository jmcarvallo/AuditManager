using System.Threading.Tasks;
using AuditManager.Application.Features.Auth.Commands.Register;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Auth;

public class RegisterModel(IAuditoriaApiClient apiClient) : PageModel
{
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Correo   { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string Cargo    { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var command = new RegisterUserCommand(Username, Correo, Password, Cargo);
            await apiClient.RegisterAsync(command);
            TempData["Success"] = "Usuario registrado correctamente. Ahora puedes iniciar sesión.";
            return RedirectToPage("/Auth/Login");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message.Contains("409") || ex.Message.Contains("Conflict")
                ? "El nombre de usuario o correo ya está registrado."
                : "Error al registrar usuario. Intenta de nuevo.";
            return Page();
        }
    }
}
