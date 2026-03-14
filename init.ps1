$ErrorActionPreference = "Stop"
$dotnet = 'C:\Program Files\dotnet\dotnet.exe'

Write-Host "Creating solution..."
& $dotnet new sln -n AuditManager

Write-Host "Creating projects..."
& $dotnet new classlib -n AuditManager.Core -f net8.0
& $dotnet new classlib -n AuditManager.Application -f net8.0
& $dotnet new classlib -n AuditManager.Infrastructure -f net8.0
& $dotnet new webapi -n AuditManager.API --use-controllers -f net8.0
& $dotnet new webapp -n AuditManager.Web -f net8.0
& $dotnet new xunit -n AuditManager.Tests -f net8.0

Write-Host "Adding projects to solution..."
& $dotnet sln add AuditManager.Core/AuditManager.Core.csproj
& $dotnet sln add AuditManager.Application/AuditManager.Application.csproj
& $dotnet sln add AuditManager.Infrastructure/AuditManager.Infrastructure.csproj
& $dotnet sln add AuditManager.API/AuditManager.API.csproj
& $dotnet sln add AuditManager.Web/AuditManager.Web.csproj
& $dotnet sln add AuditManager.Tests/AuditManager.Tests.csproj

Write-Host "Setting up references..."
& $dotnet add AuditManager.API/AuditManager.API.csproj reference AuditManager.Application/AuditManager.Application.csproj AuditManager.Infrastructure/AuditManager.Infrastructure.csproj
& $dotnet add AuditManager.Infrastructure/AuditManager.Infrastructure.csproj reference AuditManager.Core/AuditManager.Core.csproj
& $dotnet add AuditManager.Application/AuditManager.Application.csproj reference AuditManager.Core/AuditManager.Core.csproj
& $dotnet add AuditManager.Tests/AuditManager.Tests.csproj reference AuditManager.Core/AuditManager.Core.csproj AuditManager.Application/AuditManager.Application.csproj AuditManager.Infrastructure/AuditManager.Infrastructure.csproj

Write-Host "Cleaning up default templates..."
if (Test-Path AuditManager.Core/Class1.cs) { Remove-Item AuditManager.Core/Class1.cs }
if (Test-Path AuditManager.Application/Class1.cs) { Remove-Item AuditManager.Application/Class1.cs }
if (Test-Path AuditManager.Infrastructure/Class1.cs) { Remove-Item AuditManager.Infrastructure/Class1.cs }
if (Test-Path AuditManager.API/WeatherForecast.cs) { Remove-Item AuditManager.API/WeatherForecast.cs }
if (Test-Path AuditManager.API/Controllers/WeatherForecastController.cs) { Remove-Item AuditManager.API/Controllers/WeatherForecastController.cs }

Write-Host "Done!"
