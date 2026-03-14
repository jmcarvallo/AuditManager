using System;
using System.Collections.Generic;

namespace AuditManager.Core.Entities;

public class Responsable
{
    public required Guid Id { get; init; }
    public required string Nombre { get; set; }
    public required string Correo { get; set; }
    public required string Area { get; set; }
    
    // Encapsulated collection
    private readonly List<Auditoria> _auditorias = new();
    public IReadOnlyCollection<Auditoria> Auditorias => _auditorias.AsReadOnly();
}
