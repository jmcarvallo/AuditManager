using System.Collections.Generic;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuditManager.Web.Pages.Responsables;

public class IndexModel(IAuditoriaApiClient apiClient) : PageModel
{
    public List<ResponsableDto> Responsables { get; set; } = [];

    public async Task OnGetAsync()
    {
        Responsables = await apiClient.GetResponsablesAsync();
    }
}
