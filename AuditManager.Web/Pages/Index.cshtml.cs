using System.Collections.Generic;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages;

public class IndexModel(IAuditoriaApiClient apiClient) : PageModel
{
    public List<AuditoriaDto> Auditorias { get; set; } = [];

    public async Task OnGetAsync()
    {
        Auditorias = await apiClient.GetAuditoriasAsync();
    }
}
