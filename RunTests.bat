@echo off
setlocal
echo ===================================================
echo   AuditManager - Ejecutor de Pruebas Unitarias
echo ===================================================
echo.

echo [1/2] Ejecutando pruebas y generando reporte HTML...
dotnet test AuditManager.Tests\AuditManager.Tests.csproj --logger "html;LogFileName=ReportePruebas.html"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [OK] Todas las pruebas han pasado exitosamente.
) else (
    echo.
    echo [ERROR] Algunas pruebas han fallado. Revisa el reporte para mas detalles.
)

echo.
echo [2/2] Abriendo reporte en el navegador...
timeout /t 2 > nul
start AuditManager.Tests\TestResults\ReportePruebas.html

echo.
echo Proceso finalizado.
pause
