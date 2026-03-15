using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace AuditManager.Web.Handlers;

/// <summary>
/// Reads the JWT stored in the web session cookie and attaches it as Bearer
/// to every outgoing HTTP request to the API.
/// </summary>
public class TokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null)
        {
            var token = await context.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
