using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auditorias.Commands.Create;
using AuditManager.Application.Features.Auditorias.Commands.Update;
using AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;
using AuditManager.Application.Features.Hallazgos.Commands.Create;
using AuditManager.Application.Features.Responsables.Commands.Create;

namespace AuditManager.Web.Services;

public class AuditoriaApiClient(HttpClient httpClient) : IAuditoriaApiClient
{
    public async Task<List<AuditoriaDto>> GetAuditoriasAsync(Guid? responsableId = null)
    {
        var url = responsableId.HasValue
            ? $"api/auditorias?responsableId={responsableId.Value}"
            : "api/auditorias";
        return await httpClient.GetFromJsonAsync<List<AuditoriaDto>>(url) ?? new List<AuditoriaDto>();
    }

    public async Task<AuditoriaDto?> GetAuditoriaByIdAsync(Guid id)
    {
        // Notice we don't have GetById in API yet, but let's fetch all and filter for now or assume we'll add it
        var all = await GetAuditoriasAsync();
        return all.Find(a => a.Id == id);
    }

    public async Task<AuditoriaDto?> CreateAuditoriaAsync(CreateAuditoriaCommand command)
    {
        var response = await httpClient.PostAsJsonAsync("api/auditorias", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuditoriaDto>();
    }

    public async Task<bool> UpdateAuditoriaAsync(Guid id, UpdateAuditoriaCommand command)
    {
        var response = await httpClient.PutAsJsonAsync($"api/auditorias/{id}", command);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ChangeStatusAsync(Guid id, UpdateAuditoriaStatusCommand command)
    {
        var response = await httpClient.PatchAsJsonAsync($"api/auditorias/{id}/estado", command);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<ResponsableDto>> GetResponsablesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ResponsableDto>>("api/responsables") ?? new List<ResponsableDto>();
    }

    public async Task<Guid> CreateResponsableAsync(CreateResponsableCommand command)
    {
        var response = await httpClient.PostAsJsonAsync("api/responsables", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task CreateHallazgoAsync(CreateHallazgoCommand command)
    {
        var response = await httpClient.PostAsJsonAsync("api/hallazgos", command);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<HallazgoDto>> GetHallazgosByAuditoriaAsync(Guid auditoriaId)
    {
        return await httpClient.GetFromJsonAsync<List<HallazgoDto>>($"api/hallazgos?auditoriaId={auditoriaId}") ?? new List<HallazgoDto>();
    }

    public async Task DeleteHallazgoAsync(Guid hallazgoId)
    {
        var response = await httpClient.DeleteAsync($"api/hallazgos/{hallazgoId}");
        response.EnsureSuccessStatusCode();
    }
}
