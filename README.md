API Patients - .NET

**********************
**********************
SP QUE ME FALTO AÑADIR EN CODIGO:

CREATE PROCEDURE [dbo].[sp_GetPatientsByFieldCreatedAt]
    @FromDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        DocumentType,
        DocumentNumber,
        FirstName,
        LastName,
        PhoneNumber,
        Email,
        CreatedAt
    FROM Patients
    WHERE CreatedAt >= @FromDate
    ORDER BY CreatedAt DESC;
END

**********************
**********************


API REST desarrollada en .NET para la gestión de una base de datos que contiene la entidad Patients.
Permite realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) sobre los registros de pacientes.

.NET (ASP.NET Core Web API)
Entity Framework Core
SQL Server
Swagger (documentación de endpoints)

Estructura del proyecto
Controllers/ → Controladores de la API
Models/ → Modelos de datos (incluye Patient)
Data/ → Contexto de base de datos (AppDbContext)
Migrations/ → Migraciones de Entity Framework
Program.cs → Configuración principal

Requisitos previos
Antes de ejecutar el proyecto, asegúrate de tener instalado:
.NET SDK
SQL Server (local o remoto)
SQL Server Management Studio (opcional)

1. Clonar el repositorio
git clone https://github.com/felipe9121/Api_Patients.git
cd tu-repositorio

2. Configurar la cadena de conexión
En appsettings.json modifica la cadena de conexion, segun tus necesidades.
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=PatientsDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

3. Crear la base de datos
Se ejecutan estos 2 comandos en la base del proyecto:
dotnet ef migrations add InitialCreate
dotnet ef database update

4. Ejecutar la API
dotnet run
