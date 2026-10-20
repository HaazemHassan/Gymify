# Development Workflows & Change Management

This document defines the actionable workflows for creating new features, modifying existing implementations, bug fixing, and conducting verification.

---

## 1. Golden Rule Before Modifying Any Code

Before making any changes:
1. **Inspect First**: Read existing entity, handler, validator, and controller code in the affected area.
2. **Trace Dependencies**: Determine which contracts, interfaces, and profiles are affected.
3. **Minimize Scope**: Do not rewrite surrounding code, introduce speculative abstractions, or refactor unrelated files.
4. **Preserve Invariants**: Ensure existing domain constraints and security attributes remain intact.

---

## 2. Workflow 1: Creating a New Feature End-to-End

### Step 1: Domain Modeling (`src/Gymify.Domain`)
1. Create or update the entity in `Gymify.Domain/Entities/{Entity}.cs`:
   - Inherit from `FullAuditableEntity<int>` or `AuditableEntity<int>`.
   - Add private parameterless constructor for EF Core.
   - Add explicit parameterized constructor or static `Create(...)` method returning `ErrorOr<{Entity}>`.
   - Encapsulate fields with `private set;`.
   - Add explicit mutation methods returning `ErrorOr<Success>` (never `void`) with domain validation of business invariants.
2. Define repository contract in `Gymify.Domain/RepositoriesContracts/I{Entity}Repository.cs`:
   - Inherit `IGenericRepository<{Entity}>`.
3. Add repository property to `Gymify.Domain/RepositoriesContracts/IUnitOfWork.cs`.

### Step 2: Infrastructure Persistence (`src/Gymify.Infrastructure`)
1. Create entity configuration in `Gymify.Infrastructure/Data/Configurations/{Entity}Configuration.cs`:
   - Implement `IEntityTypeConfiguration<{Entity}>`.
   - Configure table name, keys, column constraints, precision, and relations.
2. Create repository implementation in `Gymify.Infrastructure/Data/Repositories/{Entity}Repository.cs`:
   - Inherit `GenericRepository<{Entity}>`, implement `I{Entity}Repository`.
3. Expose repository in `Gymify.Infrastructure/Data/Repositories/UnitOfWork.cs`.
4. Ensure `AppDbContext.cs` includes `DbSet<{Entity}>` if needed (or discovered via configurations).

### Step 3: Application CQRS (`src/Gymify.Application`)
Create feature folder under `Gymify.Application/Features/{Entity}s/`:

#### For Commands:
1. *(Shared)* `{Entity}ValidationRules.cs`: If not already existing, define centralized property rules in `Features/{Entity}s/Shared/{Entity}ValidationRules.cs`.
2. Under `Commands/{ActionName}/`:
   - `{ActionName}Command.cs`: Implement `ICommand<{ActionName}CommandResponse>` or `ICommand`. Add `[Authorize]` / `IAuthorizedRequest` / `IOwnedResourceRequest` if protected.
   - `{ActionName}CommandResponse.cs`: Define the returned DTO class.
   - `{ActionName}CommandValidator.cs`: Inherit `AbstractValidator<{ActionName}Command>`, define rules inside `ApplyValidationRules()` referencing `{Entity}ValidationRules` and `CommonValidationRules`.
   - `{ActionName}CommandHandler.cs`: Implement `ICommandHandler<{ActionName}Command, {ActionName}CommandResponse>`. Inject `IUnitOfWork`, `IDateTimeProvider` (never use `DateTime.UtcNow` / `DateTime.Now` directly), and services. Execute business logic, check `ErrorOr<Success>` results of domain mutations, call `await _unitOfWork.SaveChangesAsync(ct)`, return `ErrorOr` result.
   - *(Optional)* `Specifications/{SpecName}.cs`: If command requires a dedicated query specification, colocate it inside a `Specifications/` subfolder.
   - *(Optional)* `{ActionName}CommandCacheInvalidationPolicy.cs`: Implement `ICacheInvalidationPolicy<{ActionName}Command>` if altering cached tags.

#### For Queries:
Under `Queries/{QueryName}/`:

**A. Single-Item Query:**
1. `{QueryName}Query.cs`: Implement `IQuery<{QueryName}QueryResponse>`.
2. `{QueryName}QueryResponse.cs`: Standard class with `{ get; set; }` properties.
3. `{QueryName}QueryMappingProfile.cs`: Inherit AutoMapper `Profile`, create mapping `CreateMap<{Entity}, {QueryName}QueryResponse>()`.
4. `Specifications/{QueryName}Spec.cs`: Place any query-specific specification inside a `Specifications/` subfolder.
5. `{QueryName}QueryHandler.cs`: Implement `IQueryHandler<{QueryName}Query, {QueryName}QueryResponse>`. Use `_unitOfWork.{Entity}s.GetByIdAsync<{QueryName}QueryResponse>(id, ct)` or `GetAsync`.
6. *(Optional)* `{QueryName}QueryCachePolicy.cs`: Implement `ICachePolicy<{QueryName}Query>` if caching query results.

**B. Collection / List Query (MANDATORY PAGINATION):**
1. `{QueryName}Query.cs`: **Must inherit `PaginatedQuery`** and implement `IQuery<PaginatedResult<{QueryName}QueryResponse>>`. Add authorization markers/attributes if protected.
2. `{QueryName}QueryResponse.cs`: Standard class with `{ get; set; }` properties (never a record).
3. `{QueryName}QueryMappingProfile.cs`: AutoMapper `CreateMap<{Entity}, {QueryName}QueryResponse>()`.
4. Specifications Placement:
   - **Query-Specific Paginated Spec**: Place inside `Queries/{QueryName}/Specifications/{Entity}sPaginatedSpec.cs`. Inherits filter spec, applies `Query.OrderBy(...).Paginate(pageNumber, pageSize).AsNoTracking()`.
   - **Feature-Common Filter / Count Spec**: Place inside `Features/{Entity}s/Specifications/{Entity}sFilteredSpec.cs` (only if shared across queries).
5. `{QueryName}QueryHandler.cs`: Implement `IQueryHandler<{QueryName}Query, PaginatedResult<{QueryName}QueryResponse>>`.
   - Fetches items: `await _unitOfWork.{Entity}s.ListAsync<{QueryName}QueryResponse>(dataSpec, ct)`.
   - Fetches count: `await _unitOfWork.{Entity}s.CountAsync(countSpec, ct)`.
   - Returns: `new PaginatedResult<{QueryName}QueryResponse>(items, totalCount, request.PageNumber, request.PageSize)`.
6. API Request Mapping: Ensure `BasicPaginationRequestMappingProfile` maps `BasicPaginationRequest` ➔ `{QueryName}Query`.

### Step 4: API Layer (`src/Gymify.API`)
1. Create HTTP request models in `Gymify.API/Requests/{Entity}s/{ActionName}/`:
   - `{ActionName}Request.cs`
   - `{ActionName}RequestMappingProfile.cs` (maps Request ➔ Command/Query).
2. Create or update controller in `Gymify.API/Controllers/{Entity}sController.cs`:
   - Inherit `BaseController`.
   - For collection endpoints: Accept `[FromQuery] BasicPaginationRequest request`.
   - Map request to Command/Query via `_mapper.Map` or constructor.
   - Send via `await Mediator.Send(command/query, ct)`.
   - Return `result.IsError ? Problem(result.Errors) : Ok(result.Value)`.

---

## 3. Workflow 2: Modifying an Existing Feature

When a request asks to extend, alter, or fix existing behavior:

1. **Locate Target Files**:
   - Find the API endpoint in the corresponding Controller.
   - Find the Command or Query handler executed by MediatR.
   - Find the Domain Entity methods called by the handler.
2. **Determine Layer Impact**:
   - If adding a field to a response:
     - Update `{QueryName}QueryResponse.cs`.
     - Update `{QueryName}QueryMappingProfile.cs` if custom mapping is required.
     - If the field originates from the domain entity, verify the entity property and EF configuration.
   - If adding validation:
     - Update `{ActionName}CommandValidator.cs`.
     - Avoid performing basic field validation inside the handler.
   - If changing business rules:
     - Update the domain method inside the Entity or Domain Service first.
     - Update the Command Handler to handle any new `ErrorOr` return values.
3. **Verify Side Effects**:
   - Check if any cache tags or invalidation policies depend on the modified data.
   - Check if AutoMapper profiles for other queries or commands reference the changed properties.

---

## 4. Verification & Quality Control Checklist

Always run these checks before concluding any task:

1. **Compilation Check**:
   Run in PowerShell from the repository root:
   ```powershell
   dotnet build Gymify.sln
   ```
   Ensure:
   - `0 Error(s)`
   - `0 Warning(s)`
2. **Architecture Compliance Check**:
   - [ ] No `DbContext` injected into Application handlers.
   - [ ] No manual object mapping in queries (AutoMapper `ProjectTo` used).
   - [ ] No public property setters on Domain Entities.
   - [ ] Handlers return `ErrorOr<T>` with standard `ErrorCodes`.
   - [ ] Controller returns `Problem(result.Errors)` on failure.
   - [ ] FluentValidation validator exists for every command with user input.
   - [ ] UnitOfWork has single `SaveChangesAsync` call per command handler.
