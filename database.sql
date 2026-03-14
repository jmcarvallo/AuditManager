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
    ISNULL(SUM(CASE WHEN H.Severidad = 'Baja' THEN 1 ELSE 0 END), 0) AS HallazgosBaja,
    ISNULL(SUM(CASE WHEN H.Severidad = 'Media' THEN 1 ELSE 0 END), 0) AS HallazgosMedia,
    ISNULL(SUM(CASE WHEN H.Severidad = 'Alta' THEN 1 ELSE 0 END), 0) AS HallazgosAlta,
    COUNT(H.Id) AS TotalHallazgos
FROM 
    Auditorias A
LEFT JOIN 
    Hallazgos H ON A.Id = H.AuditoriaId
LEFT JOIN 
    Responsables R ON A.ResponsableId = R.Id
WHERE 
    A.Estado = 'Finalizada'
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
