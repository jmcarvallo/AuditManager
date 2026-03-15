using System.Net.Http.Headers;
using System.Security.Claims;

namespace AuditManager.Web.Handlers;

/// <summary>
/// Reads the JWT stored as the 'access_token' claim (set during login)
/// and attaches it as Bearer to every outgoing HTTP request to the API.
/// </summary>
public class TokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null)
        {
            var token = context.User.FindFirstValue("access_token");
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
