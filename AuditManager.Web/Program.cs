using System;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Cookie Authentication (guarda el JWT como access_token en la cookie)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath   = "/Auth/Login";
        options.LogoutPath  = "/Auth/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        // Rechaza cookies viejas que no tengan el claim access_token (ej: cookies pre-fix)
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async ctx =>
            {
                var token = ctx.Principal?.FindFirst("access_token")?.Value;
                if (string.IsNullOrEmpty(token))
                {
                    ctx.RejectPrincipal();
                    await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        };
    });

// Razor Pages con autorización global (todas las páginas requieren login)
builder.Services.AddRazorPages(options =>
{
    // Páginas públicas (no requieren login)
    options.Conventions.AllowAnonymousToPage("/Auth/Login");
    options.Conventions.AllowAnonymousToPage("/Auth/Register");
}).AddRazorPagesOptions(o => o.Conventions.AuthorizeFolder("/"));

builder.Services.AddHttpContextAccessor();

// Configure API Client
builder.Services.AddHttpClient<IAuditoriaApiClient, AuditoriaApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7197/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
