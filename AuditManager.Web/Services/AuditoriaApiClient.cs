using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auditorias.Commands.Create;
using AuditManager.Application.Features.Auditorias.Commands.Update;
using AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;

namespace AuditManager.Web.Services;

public class AuditoriaApiClient(HttpClient httpClient) : IAuditoriaApiClient
{
    public async Task<List<AuditoriaDto>> GetAuditoriasAsync()
    {
        return await httpClient.GetFromJsonAsync<List<AuditoriaDto>>("api/auditorias") ?? new List<AuditoriaDto>();
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
}
