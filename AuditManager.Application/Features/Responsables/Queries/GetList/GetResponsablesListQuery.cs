using System.Collections.Generic;
using AuditManager.Application.DTOs;
using MediatR;

namespace AuditManager.Application.Features.Responsables.Queries.GetList;

public record GetResponsablesListQuery : IRequest<List<ResponsableDto>>;
