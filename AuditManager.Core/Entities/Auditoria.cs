using System;
using System.Collections.Generic;
using AuditManager.Core.Enums;

namespace AuditManager.Core.Entities;

public class Auditoria
{
    public required Guid Id { get; init; }
    public required string Titulo { get; set; }
    public required DateTime FechaInicio { get; set; }
    public required DateTime FechaFin { get; set; }
    public required string AreaAuditada { get; set; }
    public required Guid ResponsableId { get; set; }
    public required EstadoAuditoria Estado { get; set; }

    // Navigation property
    public Responsable? Responsable { get; set; }

    // Encapsulated collection
    private readonly List<Hallazgo> _hallazgos = new();
    public IReadOnlyCollection<Hallazgo> Hallazgos => _hallazgos.AsReadOnly();

    public void AgregarHallazgo(Hallazgo hallazgo)
    {
        _hallazgos.Add(hallazgo);
    }
    
    public void EliminarHallazgo(Hallazgo hallazgo)
    {
        _hallazgos.Remove(hallazgo);
    }
}
