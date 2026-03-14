using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.Features.Hallazgos.Commands.Delete;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using Moq;
using Xunit;

namespace AuditManager.Tests.Application.Features.Hallazgos.Commands;

public class DeleteHallazgoCommandHandlerTests
{
    private readonly Mock<IRepository<Hallazgo>> _mockHallazgoRepository;
    private readonly Mock<IRepository<Auditoria>> _mockAuditoriaRepository;

    public DeleteHallazgoCommandHandlerTests()
    {
        _mockHallazgoRepository = new Mock<IRepository<Hallazgo>>();
        _mockAuditoriaRepository = new Mock<IRepository<Auditoria>>();
    }

    [Fact]
    public async Task Handle_ValidAuditoria_EnProceso_ShouldDeleteSuccess()
    {
        // Arrange
        var hallazgoId = Guid.NewGuid();
        var auditoriaId = Guid.NewGuid();
        var hallazgo = new Hallazgo 
        { 
            Id = hallazgoId, 
            AuditoriaId = auditoriaId, 
            Descripcion = "Test", 
            FechaDeteccion = DateTime.Now,
            Tipo = TipoHallazgo.NoConformidad,
            Severidad = SeveridadHallazgo.Media
        };
        var auditoria = new Auditoria 
        { 
            Id = auditoriaId, 
            Estado = EstadoAuditoria.EnProceso,
            Titulo = "Test",
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(1),
            AreaAuditada = "Test",
            ResponsableId = Guid.NewGuid()
        };

        _mockHallazgoRepository.Setup(r => r.GetByIdAsync(hallazgoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hallazgo);
        
        _mockAuditoriaRepository.Setup(r => r.GetByIdAsync(auditoriaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditoria);

        var command = new DeleteHallazgoCommand(hallazgoId);
        var handler = new DeleteHallazgoCommandHandler(_mockHallazgoRepository.Object, _mockAuditoriaRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockHallazgoRepository.Verify(r => r.DeleteAsync(It.IsAny<Hallazgo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(EstadoAuditoria.Pendiente)]
    [InlineData(EstadoAuditoria.Finalizada)]
    public async Task Handle_AuditoriaEnEstadoInvalido_ShouldThrowInvalidOperationException(EstadoAuditoria estadoInvalido)
    {
        // Arrange
        var hallazgoId = Guid.NewGuid();
        var auditoriaId = Guid.NewGuid();
        var hallazgo = new Hallazgo 
        { 
            Id = hallazgoId, 
            AuditoriaId = auditoriaId, 
            Descripcion = "Test", 
            FechaDeteccion = DateTime.Now,
            Tipo = TipoHallazgo.NoConformidad,
            Severidad = SeveridadHallazgo.Media
        };
        var auditoria = new Auditoria 
        { 
            Id = auditoriaId, 
            Estado = estadoInvalido,
            Titulo = "Test",
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(1),
            AreaAuditada = "Test",
            ResponsableId = Guid.NewGuid()
        };

        _mockHallazgoRepository.Setup(r => r.GetByIdAsync(hallazgoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hallazgo);
        
        _mockAuditoriaRepository.Setup(r => r.GetByIdAsync(auditoriaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditoria);

        var command = new DeleteHallazgoCommand(hallazgoId);
        var handler = new DeleteHallazgoCommandHandler(_mockHallazgoRepository.Object, _mockAuditoriaRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("Solo se pueden eliminar hallazgos si la auditoría se encuentra 'En Proceso'.", exception.Message);

        _mockHallazgoRepository.Verify(r => r.DeleteAsync(It.IsAny<Hallazgo>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
