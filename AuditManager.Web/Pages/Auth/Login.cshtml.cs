using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AuditManager.Application.Features.Auth.Commands.Login;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Auth;

public class LoginModel(IAuditoriaApiClient apiClient) : PageModel
{
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var result = await apiClient.LoginAsync(Username, Password);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name,  result.Username),
                new("cargo",          result.Cargo),
                new("access_token",   result.Token)
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc   = System.DateTimeOffset.UtcNow.AddHours(8)
            };
            // Store the JWT so TokenHandler can retrieve it
            props.StoreTokens([new AuthenticationToken { Name = "access_token", Value = result.Token }]);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
            return RedirectToPage("/Index");
        }
        catch
        {
            ErrorMessage = "Usuario o contraseña incorrectos.";
            return Page();
        }
    }
}
