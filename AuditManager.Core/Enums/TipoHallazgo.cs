using System.ComponentModel.DataAnnotations;

namespace AuditManager.Core.Enums;

public enum TipoHallazgo
{
    Observacion,
    [Display(Name = "No Conformidad")]
    NoConformidad
}
