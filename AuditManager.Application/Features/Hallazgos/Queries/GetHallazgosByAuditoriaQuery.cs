using System;
using System.Collections.Generic;
using AuditManager.Application.DTOs;
using MediatR;

namespace AuditManager.Application.Features.Hallazgos.Queries;

public record GetHallazgosByAuditoriaQuery(Guid AuditoriaId) : IRequest<List<HallazgoDto>>;
