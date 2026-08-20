# jestr-activity-tracking-api

API REST construida con ASP.NET Core 9 siguiendo Clean Architecture. Expone endpoints CRUD para gestionar actividades de equipo.

## Ruta local
`C:\Users\alejb\source\repos\Alejb2001\jestr-activity-tracking-api`

## Stack
- .NET 9 / ASP.NET Core 9
- Entity Framework Core 9 (SQL Server)
- Swashbuckle (Swagger UI)

## Arquitectura — 4 capas

```
src/
├── ActivityTracker.Domain/          Entidades, enums, contratos de repositorio
├── ActivityTracker.Application/     DTOs, interfaces de servicio, lógica de negocio
├── ActivityTracker.Infrastructure/  EF Core, DbContext, implementación de repositorios
└── ActivityTracker.Api/             Controladores HTTP, Program.cs, configuración
```

### Flujo de dependencias
```
Api → Application → Domain
Api → Infrastructure → Application
```
La capa Domain no depende de nada externo.

## Modelo de datos

### Entidad `Activity` (`Domain/Entities/Activity.cs`)
| Campo | Tipo | Descripción |
|---|---|---|
| Id | int | PK auto-incremental |
| Title | string (max 100) | Título de la actividad |
| Description | string (max 1000) | Descripción detallada |
| ScheduledStart | DateTime | Fecha programada de inicio |
| ScheduledEnd | DateTime | Fecha programada de conclusión |
| Status | ActivityStatus (enum) | Estado de progreso |
| AssignedUserId | string (max 100) | ID del responsable |
| CreatedAt | DateTime | Fecha de creación (UTC) |
| UpdatedAt | DateTime? | Fecha de última modificación |

### Enum `ActivityStatus` (`Domain/Enums/ActivityStatus.cs`)
- `Pending = 0`
- `InProgress = 1`
- `Completed = 2`
- `Cancelled = 3`

El enum se persiste como string en la BD (`HasConversion<string>()`).

## Endpoints

Base URL en desarrollo: `http://localhost:5000/api`

| Verbo | Ruta | Acción | Body |
|---|---|---|---|
| GET | `/api/activities` | Listar todas (orden: más reciente primero) | — |
| GET | `/api/activities/{id}` | Obtener una por ID | — |
| POST | `/api/activities` | Crear nueva | `CreateActivityDto` |
| PUT | `/api/activities/{id}` | Actualizar completa | `UpdateActivityDto` |
| DELETE | `/api/activities/{id}` | Eliminar | — |

### DTOs (`Application/DTOs/ActivityDtos.cs`)

**CreateActivityDto** (POST):
```json
{
  "title": "string",
  "description": "string",
  "scheduledStart": "2025-01-15",
  "scheduledEnd": "2025-01-20",
  "assignedUserId": "string"
}
```

**UpdateActivityDto** (PUT) — igual que Create más:
```json
{
  "status": 1
}
```

**ActivityDto** (respuesta):
```json
{
  "id": 1,
  "title": "string",
  "description": "string",
  "scheduledStart": "2025-01-15T00:00:00Z",
  "scheduledEnd": "2025-01-20T00:00:00Z",
  "status": 1,
  "statusLabel": "InProgress",
  "assignedUserId": "string",
  "createdAt": "2025-01-10T12:00:00Z",
  "updatedAt": null
}
```

## Inyección de dependencias
- `IActivityRepository` → `ActivityRepository` (Scoped, registrado en Infrastructure)
- `IActivityService` → `ActivityService` (Scoped, registrado en Application)
- Los dos proyectos exponen extension methods `AddInfrastructure()` y `AddApplication()` que se llaman en `Program.cs`

## CORS
Configurado para aceptar peticiones desde `http://localhost:4200` (dev Angular).
Policy name: `"AngularDev"`.

## Base de datos
- Proveedor: SQL Server (LocalDB en desarrollo)
- Cadena de conexión en `appsettings.json` → `ConnectionStrings:DefaultConnection`
- Default: `Server=(localdb)\mssqllocaldb;Database=ActivityTrackerDb;Trusted_Connection=True;`

### Migraciones (ejecutar desde la raíz de la solución)
```bash
dotnet ef migrations add <NombreMigracion> \
  --project src/ActivityTracker.Infrastructure \
  --startup-project src/ActivityTracker.Api

dotnet ef database update \
  --project src/ActivityTracker.Infrastructure \
  --startup-project src/ActivityTracker.Api
```

Estado actual: **migración inicial pendiente** — no se ha ejecutado aún.

## Cómo ejecutar
```bash
cd C:\Users\alejb\source\repos\Alejb2001\jestr-activity-tracking-api
dotnet run --project src/ActivityTracker.Api
```
- API: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`

## Swagger
Disponible solo en Development. Generado con Swashbuckle.AspNetCore.

## Pendientes
- [ ] Ejecutar migración inicial y crear la BD
- [ ] Agregar autenticación (JWT)
- [ ] Agregar paginación y filtros en GET /activities
- [ ] Agregar validaciones con DataAnnotations en los DTOs
- [ ] Endpoint para listar actividades por usuario asignado
