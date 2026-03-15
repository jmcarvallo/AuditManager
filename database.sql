-- database.sql
-- Script for Evaluation Requirements
-- Contains Database Creation (handled by EF conceptually, but here explicitly if needed), Views, and Functions.

USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AuditManagerDb')
BEGIN
    CREATE DATABASE [AuditManagerDb];
END
GO

USE [AuditManagerDb];
GO

-- 1. TABLA: Responsables
CREATE TABLE Responsables (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Area NVARCHAR(100) NOT NULL
);
GO

-- 2. TABLA: Auditorias
CREATE TABLE Auditorias (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Titulo NVARCHAR(200) NOT NULL,
    FechaInicio DATETIME2 NOT NULL,
    FechaFin DATETIME2 NOT NULL,
    Estado INT NOT NULL DEFAULT 0, -- 0: Pendiente, 1: EnProceso, 2: Finalizada
    AreaAuditada NVARCHAR(100) NOT NULL,
    ResponsableId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_Auditorias_Responsables FOREIGN KEY (ResponsableId) REFERENCES Responsables(Id)
);
GO

-- 3. TABLA: Hallazgos
CREATE TABLE Hallazgos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AuditoriaId UNIQUEIDENTIFIER NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    Tipo INT NOT NULL, -- 0: Observacion, 1: NoConformidadMenor, 2: NoConformidadMayor ...
    Severidad INT NOT NULL, -- 0: Baja, 1: Media, 2: Alta
    FechaDeteccion DATETIME2 NOT NULL,
    CONSTRAINT FK_Hallazgos_Auditorias FOREIGN KEY (AuditoriaId) REFERENCES Auditorias(Id) ON DELETE CASCADE
);
GO

-- 4. TABLA: Usuarios (autenticación JWT)
CREATE TABLE Usuarios (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Cargo NVARCHAR(100) NOT NULL,
    CreadoEn DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- VISTA: Auditorías finalizadas con número de hallazgos por severidad
CREATE OR ALTER VIEW vw_AuditoriasFinalizadasResumen
AS
SELECT 
    A.Id AS AuditoriaId,
    A.Titulo,
    A.FechaInicio,
    A.FechaFin,
    A.AreaAuditada,
    R.Nombre AS Responsable,
    ISNULL(SUM(CASE WHEN H.Severidad = 0 THEN 1 ELSE 0 END), 0) AS HallazgosBaja,   -- 0 = Baja
    ISNULL(SUM(CASE WHEN H.Severidad = 1 THEN 1 ELSE 0 END), 0) AS HallazgosMedia,  -- 1 = Media
    ISNULL(SUM(CASE WHEN H.Severidad = 2 THEN 1 ELSE 0 END), 0) AS HallazgosAlta,   -- 2 = Alta
    COUNT(H.Id) AS TotalHallazgos
FROM 
    Auditorias A
LEFT JOIN 
    Hallazgos H ON A.Id = H.AuditoriaId
LEFT JOIN 
    Responsables R ON A.ResponsableId = R.Id
WHERE 
    A.Estado = 2  -- 2 = Finalizada
GROUP BY 
    A.Id, A.Titulo, A.FechaInicio, A.FechaFin, A.AreaAuditada, R.Nombre;
GO

-- PROCEDIMIENTO ALMACENADO / FUNCIÓN: Resumen por fecha (como solicita el reporte endpoint)
-- El backend lo consumirá para devolver un resumen por fecha
CREATE OR ALTER PROCEDURE sp_ObtenerResumenAuditoriasPorFecha
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CAST(A.FechaFin AS DATE) AS FechaFinalizacion,
        COUNT(A.Id) AS TotalAuditorias,
        SUM(V.HallazgosBaja) AS TotalBaja,
        SUM(V.HallazgosMedia) AS TotalMedia,
        SUM(V.HallazgosAlta) AS TotalAlta
    FROM 
        Auditorias A
    INNER JOIN 
        vw_AuditoriasFinalizadasResumen V ON A.Id = V.AuditoriaId
    WHERE 
        A.FechaFin >= @FechaInicio AND A.FechaFin <= @FechaFin
    GROUP BY 
        CAST(A.FechaFin AS DATE)
    ORDER BY 
        FechaFinalizacion DESC;
END
GO
