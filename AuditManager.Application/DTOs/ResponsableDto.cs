using System;

namespace AuditManager.Application.DTOs;

public class ResponsableDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
}
