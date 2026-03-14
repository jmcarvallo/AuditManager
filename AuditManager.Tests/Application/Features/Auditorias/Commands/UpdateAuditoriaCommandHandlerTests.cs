using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auditorias.Commands.Update;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using Moq;
using Xunit;

namespace AuditManager.Tests.Application.Features.Auditorias.Commands;

public class UpdateAuditoriaCommandHandlerTests
{
    private readonly Mock<IRepository<Auditoria>> _mockAuditoriaRepository;
    private readonly Mock<IRepository<Responsable>> _mockResponsableRepository;

    public UpdateAuditoriaCommandHandlerTests()
    {
        _mockAuditoriaRepository = new Mock<IRepository<Auditoria>>();
        _mockResponsableRepository = new Mock<IRepository<Responsable>>();
    }

    [Fact]
    public async Task Handle_ValidAuditoria_EnEstadoPendiente_ShouldUpdateSuccess()
    {
        // Arrange
        var auditoriaId = Guid.NewGuid();
        var responsableId = Guid.NewGuid();
        var auditoria = new Auditoria 
        { 
            Id = auditoriaId, 
            Estado = EstadoAuditoria.Pendiente,
            Titulo = "Old",
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(1),
            AreaAuditada = "Old",
            ResponsableId = responsableId
        };
        var responsable = new Responsable 
        { 
            Id = responsableId,
            Nombre = "John Doe",
            Correo = "john@example.com",
            Area = "IT"
        };

        _mockAuditoriaRepository.Setup(r => r.GetByIdAsync(auditoriaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditoria);
        
        _mockResponsableRepository.Setup(r => r.GetByIdAsync(responsableId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(responsable);

        var command = new UpdateAuditoriaCommand(auditoriaId, "Updated", DateTime.Now, DateTime.Now.AddDays(1), "Updated Area", responsableId);
        var handler = new UpdateAuditoriaCommandHandler(_mockAuditoriaRepository.Object, _mockResponsableRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Titulo);
        _mockAuditoriaRepository.Verify(r => r.UpdateAsync(It.IsAny<Auditoria>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(EstadoAuditoria.EnProceso)]
    [InlineData(EstadoAuditoria.Finalizada)]
    public async Task Handle_AuditoriaEnEstadoInvalido_ShouldThrowInvalidOperationException(EstadoAuditoria estadoInvalido)
    {
        // Arrange
        var auditoriaId = Guid.NewGuid();
        var responsableId = Guid.NewGuid();
        var auditoria = new Auditoria 
        { 
            Id = auditoriaId, 
            Estado = estadoInvalido,
            Titulo = "Old",
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(1),
            AreaAuditada = "Old",
            ResponsableId = responsableId
        };

        _mockAuditoriaRepository.Setup(r => r.GetByIdAsync(auditoriaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditoria);

        var command = new UpdateAuditoriaCommand(auditoriaId, "Updated", DateTime.Now, DateTime.Now.AddDays(1), "Updated Area", responsableId);
        var handler = new UpdateAuditoriaCommandHandler(_mockAuditoriaRepository.Object, _mockResponsableRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("Solo se puede actualizar una auditoría en estado Pendiente.", exception.Message);
        
        _mockAuditoriaRepository.Verify(r => r.UpdateAsync(It.IsAny<Auditoria>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
