using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.Features.Hallazgos.Commands.Create;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using Moq;
using Xunit;

namespace AuditManager.Tests.Application.Features.Hallazgos.Commands;

public class CreateHallazgoCommandHandlerTests
{
    private readonly Mock<IRepository<Hallazgo>> _mockHallazgoRepository;
    private readonly Mock<IRepository<Auditoria>> _mockAuditoriaRepository;

    public CreateHallazgoCommandHandlerTests()
    {
        _mockHallazgoRepository = new Mock<IRepository<Hallazgo>>();
        _mockAuditoriaRepository = new Mock<IRepository<Auditoria>>();
    }

    [Fact]
    public async Task Handle_ValidAuditoria_EnProceso_ShouldCreateSuccess()
    {
        // Arrange
        var auditoriaId = Guid.NewGuid();
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

        _mockAuditoriaRepository.Setup(r => r.GetByIdAsync(auditoriaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditoria);
        
        _mockHallazgoRepository.Setup(r => r.AddAsync(It.IsAny<Hallazgo>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Hallazgo h, CancellationToken c) => h);

        var command = new CreateHallazgoCommand(auditoriaId, "Falta control de acceso", TipoHallazgo.NoConformidad, SeveridadHallazgo.Alta, DateTime.Now);
        var handler = new CreateHallazgoCommandHandler(_mockHallazgoRepository.Object, _mockAuditoriaRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _mockHallazgoRepository.Verify(r => r.AddAsync(It.IsAny<Hallazgo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AuditoriaNoExiste_ShouldThrowException()
    {
        // Arrange
        _mockAuditoriaRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Auditoria?)null);

        var command = new CreateHallazgoCommand(Guid.NewGuid(), "Falta control de acceso", TipoHallazgo.NoConformidad, SeveridadHallazgo.Alta, DateTime.Now);
        var handler = new CreateHallazgoCommandHandler(_mockHallazgoRepository.Object, _mockAuditoriaRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("La auditoría especificada no existe.", exception.Message);

        _mockHallazgoRepository.Verify(r => r.AddAsync(It.IsAny<Hallazgo>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
