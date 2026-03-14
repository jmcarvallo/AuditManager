# Audit Manager System

Este repositorio contiene la solución a la Evaluación Técnica para Full Stack Senior .NET.

## Arquitectura y Tecnologías
La solución está desarrollada en **.NET 8** utilizando los principios de **Clean Architecture** y los patrones **CQRS** (con MediatR) y Repository.

### Capas del Proyecto:
* **AuditManager.Core**: Entidades del dominio (`Auditoria`, `Hallazgo`, `Responsable`) y Enumeradores. No tiene dependencias.
* **AuditManager.Application**: Lógica de aplicación, casos de uso (MediatR), validaciones (FluentValidation) y DTOs.
* **AuditManager.Infrastructure**: Acceso a datos utilizando Entity Framework Core (Code-First approach para estructura base) complementado por scripts SQL directos para Vistas y Funciones Almacenadas, operando sobre SQL Server.
* **AuditManager.API**: Aplicación RESTful basada en .NET 8 Web API, con Swagger, mapeo global de excepciones y endpoints expuestos.
* **AuditManager.Web**: Frontend desarrollado con ASP.NET Core Razor Pages consumiendo la API mediante `HttpClient`. Interfaz estilizada con Bootstrap 5.
* **AuditManager.Tests**: Proyecto xUnit utilizando `Moq` para probar rigurosamente las reglas de negocio en los Command Handlers.

## Requisitos de Ejecución
1. .NET 8 SDK
2. SQL Server (LocalDB o Express)
3. Ejecutar el script `database.sql` (en la raíz) contra la base de datos `AuditoriasDB` (el script asume que la DB ya está creada o crea las tablas en el default catalog, asegúrese de apuntar la conexión a `AuditoriasDB`).
4. **Cadena de conexión**: En `AuditManager.API/appsettings.json`, ajustar la cadena de conexión.

## Validaciones y Reglas de Negocio Implementadas
* Se utiliza FluentValidation para validar entidades en la API.
* Una Auditoría únicamente se puede editar (modificar o agregar hallazgos) si está en estado **`Pendiente`** o **`EnProceso`** según el flujo correspondiente:
    * Modificaciones generales (Título, Área, Fecha): Sólo `Pendiente`.
    * Creación y Eliminación de Hallazgos: Sólo con Auditoría `EnProceso`.
* Estas reglas están estrictamente testeadas en la capa de Aplicación.

## Entregables Adicionales
* **Postman Collection**: El archivo `Auditorias_Postman.json` contiene la colección de requests configurados.
* **Script de BD**: `database.sql` con creación de tablas, la vista `vw_EstadoAuditorias` y la función `fn_ObtenerTotalesAuditoria`.
* **Ramas Git**: La estructura respeta *main* y *develop* (GitFlow format).
