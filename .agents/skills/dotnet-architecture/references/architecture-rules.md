# Architectural Rules & Constraints

This document defines the strict architectural boundaries, dependency rules, and invariants that must be maintained across the solution.

---

## 1. Solution Architecture & Project Boundaries

The solution is divided into four concentric layers following Clean Architecture and Domain-Driven Design (DDD):

```
┌────────────────────────────────────────────────────────┐
│                      Gymify.API                        │
│   (Controllers, HTTP Requests, Swagger, Middlewares)    │
└──────────────────────────┬─────────────────────────────┘
                           │ references
                           ▼
┌────────────────────────────────────────────────────────┐
│                  Gymify.Application                    │
│     (CQRS, Behaviors, Validation, Caching Policies,    │
│      AutoMapper Profiles, Specifications, DTOs)        │
└──────────────────────────┬─────────────────────────────┘
                           │ references
                           ▼
┌────────────────────────────────────────────────────────┐
│                     Gymify.Domain                      │
│   (Entities, Value Objects, Domain Events, Constants,   │
│    Enums, Repository Contracts, Unit of Work Contract) │
└──────────────────────────▲─────────────────────────────┘
                           │ referenced by
┌──────────────────────────┴─────────────────────────────┐
│                 Gymify.Infrastructure                  │
│     (EF Core AppDbContext, Repositories, UnitOfWork,   │
│      Redis/HybridCache, External Services, Outbox)     │
└────────────────────────────────────────────────────────┘
```

### Dependency Invariants
- **`Gymify.Domain`**: Must **never** reference any other project in the solution. It contains pure enterprise and business logic with zero framework dependencies (except lightweight primitives like `ErrorOr` and `Ardalis.Specification` contract interfaces where applicable).
- **`Gymify.Application`**: Only references `Gymify.Domain`. It contains MediatR handlers, validation rules, mapping profiles, and contracts. It must **never** reference `Gymify.Infrastructure` or `Gymify.API`.
- **`Gymify.Infrastructure`**: References `Gymify.Application` and `Gymify.Domain`. It implements persistence, external service contracts, and low-level data access.
- **`Gymify.API`**: References `Gymify.Application` and `Gymify.Infrastructure`. It handles HTTP transport, model binding, and dependency injection wiring.

---

## 2. Classification of Standards

To prevent cargo-culting and inappropriate code bloat, all practices are categorized into four distinct levels:

### Level A: Mandatory Architectural Constraints (Never Violate)
1. **Clean Dependency Direction**: Inner layers must never know about outer layers.
2. **Encapsulated Domain Models**: Domain entities must never expose public setters on business properties. All mutations must occur via explicit domain methods or factory methods.
3. **No Unhandled Business Exceptions**: Business and validation failures must **always** return `ErrorOr<T>` with standard `Error` types (`Error.Validation`, `Error.NotFound`, `Error.Conflict`, `Error.Unauthorized`, `Error.Forbidden`). Exceptions are reserved strictly for unexpected technical failures.
4. **AutoMapper Projections for Query Performance**: Handlers fetching data for queries must use EF Core `ProjectTo` via `GenericRepository<T>` methods (`ListAsync<TDto>(spec)`, `GetByIdAsync<TDto>(id)`, `GetAsync<TDto>(spec)`). **Manual property-by-property object instantiation from tracked entities in query handlers is strictly prohibited.**
5. **No Direct EF DbContext in Application Layer**: Handlers must interact with persistence exclusively through `IUnitOfWork` and repository contracts (`IGenericRepository<T>`).
6. **Controllers Inherit `BaseController`**: Action methods must dispatch requests via `Mediator.Send(...)`, check `result.IsError`, and return `Problem(result.Errors)` or `Ok(result.Value)`.
7. **Strict CQRS File & Class Naming Alignment**: Every supporting class in a command or query folder must strictly reflect the request name:
   - For `{CommandName}`: `{CommandName}.cs`, `{CommandName}Handler.cs`, `{CommandName}Response.cs`, `{CommandName}Validator.cs`, `{CommandName}CacheInvalidationPolicy.cs`.
   - For `{QueryName}`: `{QueryName}.cs`, `{QueryName}Handler.cs`, `{QueryName}Response.cs`, `{QueryName}MappingProfile.cs`, `{QueryName}CachePolicy.cs`.
   - *Prohibited*: Generic names like `PackageManagementResponse` for `GetPackagesForManagementQuery`. It MUST be `GetPackagesForManagementQueryResponse`.
8. **Responses Must Be Classes, Not Records**: DTOs returned by Commands and Queries must be standard `class` types with public `{ get; set; }` properties. **Do NOT use `record` for responses**, as AutoMapper projections (`ProjectTo`) require parameterless constructors and property setters to build efficient SQL expression trees.
9. **Mandatory Pagination for Collection/List Queries**: Any query that retrieves a collection or list of items must inherit from `PaginatedQuery` and return `IQuery<PaginatedResult<TResponse>>`. **Never return unpaginated lists (`List<T>`, `IEnumerable<T>`) from queries**. Handlers must apply `.Paginate(pageNumber, pageSize)` via Ardalis specifications and execute `CountAsync(countSpec)` to return `new PaginatedResult<TResponse>(items, totalCount, pageNumber, pageSize)`. API endpoints must accept `[FromQuery] BasicPaginationRequest request`.
10. **Validation Rules Single Source of Truth**: Command validators within a feature must not write duplicate inline validation rules (such as string lengths, numeric ranges, or regex patterns). All property rules must be centralized in a `{Entity}ValidationRules` static class under `Features/{Entity}s/Shared/` (or `CommonValidationRules.cs` for cross-feature primitives like Email, Phone, Address, Required). Command validators must invoke `ApplyValidationRules()` in their constructor.
11. **Specification Colocation Hierarchy**:
    - **Query/Command-Specific Specs**: Must be placed in a `Specifications/` subfolder *inside* that specific query or command directory (e.g., `Features/{Entity}s/Queries/{QueryName}/Specifications/{SpecName}.cs`).
    - **Feature-Common Specs**: Only specifications shared across multiple queries or commands within the feature belong in `Features/{Entity}s/Specifications/{CommonSpecName}.cs`.
    - **Global Shared Specs**: Generic specifications across aggregates belong in `Gymify.Application/Specifications/Shared/` (e.g., `GetByIdSpec<T>`).
12. **Mandatory Use of `IDateTimeProvider` via Dependency Injection**:
    - **Never** use `DateTime.UtcNow`, `DateTime.Now`, `DateTimeOffset.UtcNow`, or `DateTimeOffset.Now` directly in command handlers, query handlers, or domain orchestration services.
    - **Always** inject `IDateTimeProvider` (from `Gymify.Application.ServicesContracts.Infrastructure`) via constructor and retrieve timestamps through `_dateTimeProvider.UtcNow`. This ensures absolute testability, clock consistency, and predictable timezone conversion.
13. **Domain Mutation Methods Must Return `ErrorOr<Success>` with Domain Validation**:
    - Entity methods that mutate state or transition lifecycles must **never** return `void`.
    - They must return `ErrorOr<Success>` (or `ErrorOr<T>`) and encapsulate domain business invariants (e.g., checking lifecycle prerequisites, verifying logical time order, preventing invalid re-execution).
    - Handlers invoking these methods must check `if (result.IsError) return result.Errors;` before calling `SaveChangesAsync`. This keeps business logic and validation rich within the Domain layer rather than leaking into application handlers.
14. **Mandatory Use of `DateTimeOffset` Over `DateTime`**:
    - **Always** declare timestamps, dates, expiration times, and event occurrence properties using `DateTimeOffset` (and `DateTimeOffset?`), **never** raw `DateTime`.
    - `DateTimeOffset` eliminates timezone and daylight saving ambiguities, guarantees UTC offset awareness, and matches `IDateTimeProvider.UtcNow` and EF Core `datetimeoffset` column types directly.
15. **Strict Prohibition of Hardcoded Error Code Strings**:
    - **Never** use hardcoded string literals when constructing `Error` objects (e.g. `Error.Conflict("Locker.AlreadyActive", ...)` is strictly prohibited).
    - **Always** declare error code constants as `public const string` inside `Gymify.Domain.Common.Constants.ErrorCodes` grouped by aggregate/feature class (e.g. `ErrorCodes.Locker.AlreadyActive`), and reference them universally across entities, handlers, and tests.
16. **Restricted Use of Route Names (`RouteNames.cs`)**:
    - **Do NOT add constants in `RouteNames.cs` for every endpoint indiscriminately**.
    - Route names in `RouteNames.cs` and `[HttpGet(..., Name = ...)]` are reserved **strictly** for endpoints that need to be referenced by name in HTTP responses — specifically `GetById` endpoints referenced by `CreatedAtRoute` in entity creation endpoints (e.g., `RouteNames.Packages.GetPackageByIdForManagement`, `RouteNames.Users.GetUserById`).
    - Standard query endpoints (such as paginated list queries, search queries, status checks, current occupancy, etc.) and mutation endpoints must **never** have route names assigned to them.
    - If a creation action does not have a dedicated `GetById` endpoint to return a URL for, return `Ok(result.Value)` or `StatusCode(StatusCodes.Status201Created, result.Value)` instead of linking to an unrelated list query or inventing superfluous route names.
17. **Explicit Composite Entity & User IDs in Customer and Staff DTOs**:
    - **Never return only a single ambiguous `Id`** when returning Customer, Trainer, or Employee responses.
    - **For Customers**: Always return both `CustomerId` and `UserId` (along with `Id` = `CustomerId` for backward compatibility).
    - **For Trainers**: Always return `TrainerId`, `EmployeeId`, and `UserId` (along with `Id` = `TrainerId`).
    - **For Other Staff/Employees**: Always return `EmployeeId` and `UserId` (along with `Id` = `EmployeeId` or specific role ID).
    - Frontend clients and consumer services routinely need to correlate the underlying authentication/domain user account with the specific profile entity (Customer, Employee, Trainer) for subscriptions, attendances, payroll, and scheduling. Returning all relevant IDs avoids multiple redundant roundtrips.
18. **Mandatory Audit Timestamps (`CreatedAt`, `UpdatedAt`) in Administrative & Management DTOs**:
    - Any query response or DTO returned for management, staff, or administrative list/item views (e.g., packages for management, customers list, lockers list, subscriptions, employee management) must return audit timestamps:
      - `public DateTimeOffset CreatedAt { get; set; }`
      - `public DateTimeOffset? UpdatedAt { get; set; }`
    - Staff and administrators require direct visibility into when records were created or last modified for auditing, tracking, and sorting purposes.
    - Always use `DateTimeOffset` and `DateTimeOffset?` (never raw `DateTime`).

### Level B: Established Implementation Conventions (Follow When Applicable)
1. **CQRS Segregation**: Separate operations into `Commands` (state-modifying) and `Queries` (read-only) under feature-specific directories.
2. **Validator Proximity**: Put `AbstractValidator<TCommand>` in the same folder as the command it validates.
3. **Specification Encapsulation**: Encapsulate filtering, sorting, and joins in Ardalis `Specification<T>` classes rather than writing ad-hoc LINQ expression trees in handlers.
4. **Request DTOs in API Layer**: Define HTTP requests in `Gymify.API/Requests/{Feature}/{Action}` and map to commands/queries via AutoMapper profiles (`CreateMap<TRequest, TCommand>()`).

### Level C: Optional Components (Introduce Only When Required)
1. **Caching (`ICachePolicy<T>`)**: Add only to queries that are read-heavy, performance-sensitive, and tolerate short caching windows.
2. **Cache Invalidation (`ICacheInvalidationPolicy<T>`)**: Add only to commands that modify data cached by queries with tags.
3. **Domain Events (`IDomainEvent`)**: Introduce only when state transitions in an aggregate must notify other aggregates or trigger side effects asynchronously.
4. **Dedicated Specifications**: For simple primary key lookups, use `_unitOfWork.Repo.GetByIdAsync(id)` or `GetByIdSpec<T>`. A custom specification is only needed when filtering, including relations, or sorting.

### Level D: Feature-Specific Decisions (Do Not Blindly Replicate)
1. **User Type / Staff Policies**: Features like branch staff permissions (`[BranchAuthorize]`) or multi-tenant filters were specific to PrimeFit gyms. Do not invent custom authorization attributes unless the domain explicitly demands them; use standard `[Authorize]`, `IAuthorizedRequest`, or `IOwnedResourceRequest`.
2. **Custom Domain Services**: Only create domain services when business logic spans multiple aggregates and cannot naturally live inside a single entity.

---

## 3. Anti-Patterns to Strictly Avoid

| Anti-Pattern | Why It Is Prohibited | Correct Pattern |
| :--- | :--- | :--- |
| **Unpaginated List Queries** (`IQuery<List<TResponse>>`) | Causes unbound memory consumption, database exhaustion, and latency spikes as tables grow. | Inherit from `PaginatedQuery`, return `IQuery<PaginatedResult<TResponse>>`, and apply `Query.Paginate(pageNumber, pageSize)` with a filtered count spec. |
| **Inline Duplicate Validation Rules** | Violates DRY, creates drift where different commands for the same aggregate enforce conflicting constraints. | Define property rules once in `{Entity}ValidationRules` in `Shared/` and call from `ApplyValidationRules()`. |
| **Dumping Query Specs into Feature Root** | Clutters feature namespace, pollutes discoverability, and hides query-specific ownership. | Place query-specific specs inside `Queries/{QueryName}/Specifications/`; keep feature root `Specifications/` strictly for common specs. |
| **Manual Query Mapping** (`new MyDto { Id = e.Id, ... }`) | Bypasses database projections, fetches all columns, causes severe over-fetching and N+1 queries. | Use AutoMapper `CreateMap<TEntity, MyDto>()` and repository `.ListAsync<MyDto>(spec)`. |
| **Using `record` for Response DTOs** (`public record MyResponse(...)`) | Causes expression tree translation failures in AutoMapper `ProjectTo` and hinders mapping flexibility. | Declare response types as standard classes: `public class MyResponse { public int Id { get; set; } }`. |
| **Mismatched CQRS Artifact Names** (e.g. `PackageCustomerResponse` for `GetPackagesForCustomersQuery`) | Breaks codebase consistency, navigation, and automated architectural discovery. | Always align the class and file names to the request: `{RequestName}Response`, `{RequestName}Handler`, `{RequestName}Validator`, `{RequestName}MappingProfile`. |
| **Public Entity Setters** (`public string Name { get; set; }`) | Violates DDD encapsulation, allows entities to enter invalid states from any layer. | Use `private set;` with factory `Create(...)` or explicit mutation methods like `UpdateDetails(...)`. |
| **Throwing Exceptions for Business Errors** (`throw new NotFoundException(...)`) | Destroys performance, pollutes stack traces, and violates the functional result pattern. | Return `Error.NotFound(code, description)` with `ErrorOr<T>`. |
| **Logic in Controllers** | Breaks testability and violates single responsibility. | Controllers only bind requests, invoke `Mediator.Send(...)`, and map result to `ActionResult`. |
| **Calling SaveChangesAsync Multiple Times** | Destroys transaction atomicity and increases latency. | Call `await _unitOfWork.SaveChangesAsync(ct)` once per command handler. |
| **Bypassing UnitOfWork** | Creates untracked contexts and inconsistency. | Always inject `IUnitOfWork` into handlers. |
| **Direct `DateTime.UtcNow` / `DateTime.Now` Usage** | Destroys testability, leads to flaky time-dependent tests, and prevents uniform time-zone control. | Inject `IDateTimeProvider` via constructor and use `_dateTimeProvider.UtcNow`. |
| **Void Domain Mutation Methods Lacking Validation** (`public void CheckOut(...)`) | Bypasses domain invariant validation and allows entities to enter corrupt or contradictory states silently. | Return `ErrorOr<Success>` from domain methods, validate business prerequisites, and verify `result.IsError` in calling handlers. |
| **Using `DateTime` Instead of `DateTimeOffset`** | Introduces timezone ambiguity, corrupts local-vs-UTC conversions, and leads to inconsistent timestamps. | Use `DateTimeOffset` (and `DateTimeOffset?`) for all date and time properties across entities and DTOs. |
| **Hardcoding Error Codes as Strings** (`Error.Conflict("Locker.AlreadyActive", ...)`) | Destroys type safety, causes drift between client and server, and breaks centralized error cataloging. | Define all error codes as `public const string` in `ErrorCodes.cs` under the domain class and reference `ErrorCodes.{Domain}.{Constant}`. |
| **Indiscriminate Route Names for All Endpoints** (`RouteNames.Lockers.GetLockers`, `[HttpGet(Name = ...)]`) | Pollutes `RouteNames.cs`, creates useless coupling, and breaks REST conventions. | Only define route names in `RouteNames.cs` for endpoints required by `CreatedAtRoute` (primarily `GetById`). Normal endpoints do not need route names. |
| **Omitting Audit Timestamps from Management Responses** | Prevents staff/admins from tracking when records were created or modified. | Always include `DateTimeOffset CreatedAt` and `DateTimeOffset? UpdatedAt` on administrative and management list and item DTOs. |
