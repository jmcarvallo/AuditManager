using AuditManager.Application.Features.Auditorias.Commands.Create;
using FluentValidation;

namespace AuditManager.Application.Features.Auditorias.Commands.Create;

public class CreateAuditoriaCommandValidator : AbstractValidator<CreateAuditoriaCommand>
{
    public CreateAuditoriaCommandValidator()
    {
        RuleFor(p => p.Titulo)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(200).WithMessage("{PropertyName} no debe exceder {MaxLength} caracteres.");

        RuleFor(p => p.AreaAuditada)
            .NotEmpty().WithMessage("{PropertyName} es requerida.");

        RuleFor(p => p.FechaFin)
            .GreaterThanOrEqualTo(p => p.FechaInicio)
            .WithMessage("La Fecha Fin debe ser mayor o igual a la Fecha Inicio.");
    }
}
