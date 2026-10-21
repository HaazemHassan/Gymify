---
name: dotnet-architecture
description: Comprehensive architectural guide and standard operating procedure for developing, modifying, refactoring, and verifying features in the Gymify/PrimeFit .NET Clean Architecture backend template. Covers CQRS (MediatR), Ardalis Specification, AutoMapper database projections, FluentValidation, ErrorOr result pattern, Unit of Work, and caching policies.
---

# Gymify & PrimeFit .NET Architecture Skill

This skill is the definitive architectural blueprint and operational manual for any AI agent working with this .NET backend solution template. It guides both **creating new features** and **modifying, extending, refactoring, or fixing existing code**.

---

## 1. Solution Architecture at a Glance

The solution is organized in four strict concentric layers:

```
[Gymify.API]           ──▶ HTTP transport, BaseController, Request DTOs & Mapping Profiles
     │ (references)
     ▼
[Gymify.Application]   ──▶ CQRS (MediatR), Pipeline Behaviors, AutoMapper, FluentValidation, Specifications
     │ (references)
     ▼
[Gymify.Domain]        ──▶ Entities, Value Objects, Domain Events, ErrorCodes, Repository & UnitOfWork Contracts
     ▲ (referenced by)
     │
[Gymify.Infrastructure]──▶ EF Core AppDbContext, Repositories, UnitOfWork, Redis/HybridCache, Outbox
```

### MediatR Pipeline Execution Sequence
Requests entering `Mediator.Send(...)` pass through these registered pipeline behaviors in exact order:
1. `TrimmingBehavior` (trims inbound string properties)
2. `LoggingBehavior` (tracks execution duration & errors)
3. `AuthorizationBehavior` (evaluates `[Authorize]`, `IAuthorizedRequest`, `IOwnedResourceRequest`)
4. `ValidationBehavior` (executes FluentValidation rules; returns `Error.Validation` on failure)
5. `IdempotentCommandPipelineBehavior` (handles idempotent execution)
6. `CachingBehavior` (checks `ICachePolicy<T>` and returns cached data)
7. `CacheInvalidationBehavior` (invalidates tags via `ICacheInvalidationPolicy<T>` on success)
8. `PostCommitBehavior` (runs post-commit handlers)
9. `TransactionBehavior` (wraps modifying commands in a database transaction)

---

## 2. Standards Classification: Rules vs Patterns

Before writing or modifying any code, recognize the tier of each standard:

| Classification | Meaning | Examples |
| :--- | :--- | :--- |
| **Mandatory Rules** | **Never violate.** Core architectural constraints. | Dependency direction, encapsulated entity setters, returning `ErrorOr<T>` instead of business exceptions, domain mutation methods returning `ErrorOr<Success>` with domain validations, mandatory use of `IDateTimeProvider` via DI (never direct `DateTime.UtcNow`/`DateTime.Now`), mandatory use of `DateTimeOffset` over `DateTime` for all timestamps, strict prohibition of hardcoded error strings (always define in and reference from `ErrorCodes.{Domain}`), using AutoMapper `ProjectTo` for query projections, controllers inheriting `BaseController`, strict CQRS naming alignment (`{RequestName}Response`, etc.), declaring response models as `class` (never `record`), mandatory pagination for all list/collection queries (`PaginatedQuery` returning `PaginatedResult<T>`), single source of truth for validation rules (`{Entity}ValidationRules` in `Shared/`), query/command-specific specifications colocated in `Specifications/` subfolders, restricted use of `RouteNames.cs` strictly for `GetById` endpoints referenced by `CreatedAtRoute`, mandatory return of explicit composite IDs (`CustomerId` + `UserId` for customers, `TrainerId` + `EmployeeId` + `UserId` for trainers/staff), and mandatory inclusion of audit timestamps (`CreatedAt`, `UpdatedAt` as `DateTimeOffset`) in administrative/management list and item DTOs. |
| **Established Conventions** | **Standard practice.** Follow when applicable. | CQRS separation into `Commands/` and `Queries/`, colocated `AbstractValidator<T>` calling `ApplyValidationRules()`, feature-level `Specifications/` folder exclusively for common specifications shared across multiple queries/commands. |
| **Optional Components** | **Introduce only when required.** Do not force into every feature. | `ICachePolicy<T>` (only for high-read queries), `ICacheInvalidationPolicy<T>` (only for commands invalidating caches), `IDomainEvent` (only for cross-aggregate side effects). |
| **Feature-Specific Decisions** | **Context-dependent.** Do not blindly replicate. | Custom authorization attributes (e.g. legacy `[BranchAuthorize]`), specific multi-tenant filters, or domain services spanning multiple aggregates. |

For the complete list of rules and anti-patterns, consult [architecture-rules.md](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/architecture-rules.md).

---

## 3. Permanent Reference Implementations

Whenever creating or modifying a feature, consult these concrete reference implementations in the codebase first:

### Primary Command Reference: `Users.UpdateProfile`
Demonstrates an authenticated command updating an existing entity with domain mutation, uniqueness check, FluentValidation using shared rules, and `ErrorOr` handling:
- **Request**: [UpdateProfileRequest.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Requests/Users/UpdateProfile/UpdateProfileRequest.cs)
- **Request Profile**: [UpdateProfileRequestMappingProfile.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Requests/Users/UpdateProfile/UpdateProfileRequestMappingProfile.cs)
- **Command**: [UpdateProfileCommand.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommand.cs)
- **Validator**: [UpdateProfileCommandValidator.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommandValidator.cs) (calls `ApplyValidationRules()` backed by [UserValidationRules.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Shared/UserValidationRules.cs))
- **Handler**: [UpdateProfileCommandHandler.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommandHandler.cs)
- **Response**: [UpdateProfileCommandResponse.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommandResponse.cs)
- **Controller Action**: [UsersController.cs (lines 50-65)](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Controllers/UsersController.cs#L50-L65)

### Primary Single-Item Query Reference: `Customers.GetCustomerMe`
Demonstrates a clean CQRS query using a query-specific Ardalis Specification located in `Specifications/` and AutoMapper projection without manual property mapping:
- **Query**: [GetCustomerMeQuery.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/GetCustomerMeQuery.cs)
- **Query-Specific Specification**: [CustomerByDomainUserIdSpec.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/Specifications/CustomerByDomainUserIdSpec.cs)
- **Mapping Profile**: [CustomerMeMappingProfile.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/CustomerMeMappingProfile.cs)
- **Response**: [CustomerMeResponse.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/CustomerMeResponse.cs)
- **Handler**: [GetCustomerMeQueryHandler.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/GetCustomerMeQueryHandler.cs)
- **Controller Action**: [CustomersController.cs (lines 10-21)](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Controllers/CustomersController.cs#L10-L21)

### Primary Paginated List Query Reference: `Packages.GetPackagesForCustomers` / `GetPackagesForManagement`
Demonstrates a paginated collection query using `PaginatedQuery`, query-specific Ardalis Specification in `Specifications/`, common count specification, `PaginatedResult<T>`, and `BasicPaginationRequest`:
- **Query**: [GetPackagesForCustomersQuery.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Packages/Queries/GetPackagesForCustomers/GetPackagesForCustomersQuery.cs) (inherits `PaginatedQuery`, implements `IQuery<PaginatedResult<GetPackagesForCustomersQueryResponse>>`)
- **Query-Specific Specification**: [ActivePackagesPaginatedSpec.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Packages/Queries/GetPackagesForCustomers/Specifications/ActivePackagesPaginatedSpec.cs)
- **Common Filtered / Count Specification**: [PackagesFilteredSpec.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Packages/Specifications/PackagesFilteredSpec.cs)
- **Shared Validation Rules**: [PackageValidationRules.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Packages/Shared/PackageValidationRules.cs)
- **Mapping Profile**: [GetPackagesForCustomersQueryMappingProfile.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Packages/Queries/GetPackagesForCustomers/GetPackagesForCustomersQueryMappingProfile.cs)
- **Response**: [GetPackagesForCustomersQueryResponse.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Packages/Queries/GetPackagesForCustomers/GetPackagesForCustomersQueryResponse.cs)
- **Handler**: [GetPackagesForCustomersQueryHandler.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Packages/Queries/GetPackagesForCustomers/GetPackagesForCustomersQueryHandler.cs)
- **API Request Mapping**: [BasicPaginationRequestMappingProfile.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Requests/Common/Pagination/BasicPaginationRequestMappingProfile.cs)
- **Controller Actions**: [PackagesController.cs (lines 28-59)](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Controllers/PackagesController.cs#L28-L59)

### Companion Reference: Caching & Domain Events
For features requiring query caching (`ICachePolicy<T>`), cache invalidation (`ICacheInvalidationPolicy<T>`), or cross-aggregate domain events (`IDomainEvent`), consult the complete self-contained implementations documented in:
- [reference-implementations.md#3-companion-advanced-reference-caching--domain-events-self-contained](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/reference-implementations.md)
- [patterns-and-components.md#8-caching--cache-invalidation](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/patterns-and-components.md#8-caching--cache-invalidation)

---

## 4. Universal 7-Step Development Workflow

Whether creating a new feature or modifying existing code, execute this workflow:

```
┌─────────────────────────────────┐
│ 1. Understand & Inspect Target  │ ──▶ Read existing entity, handler, validator, controller
└────────────────┬────────────────┘
                 ▼
┌─────────────────────────────────┐
│ 2. Trace Layer Dependencies     │ ──▶ Identify impacted contracts, entities, DTOs, profiles
└────────────────┬────────────────┘
                 ▼
┌─────────────────────────────────┐
│ 3. Consult Reference Feature    │ ──▶ Check Users.UpdateProfile or Customers.GetCustomerMe
└────────────────┬────────────────┘
                 ▼
┌─────────────────────────────────┐
│ 4. Determine Minimal Scope      │ ──▶ No unnecessary files or unrelated refactorings
└────────────────┬────────────────┘
                 ▼
┌─────────────────────────────────┐
│ 5. Implement Across Layers      │ ──▶ Domain ➔ Infrastructure ➔ Application ➔ API
└────────────────┬────────────────┘
                 ▼
┌─────────────────────────────────┐
│ 6. Compile & Verify Build       │ ──▶ Run `dotnet build Gymify.sln`
└────────────────┬────────────────┘
                 ▼
┌─────────────────────────────────┐
│ 7. Quality & Regression Review  │ ──▶ Check error mappings, invariants, projections
└─────────────────────────────────┘
```

For step-by-step instructions on each layer, see [workflows.md](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/workflows.md).

---

## 5. Implementation Quick Cheatsheet

### A. Creating a Command
1. **Command**:
   ```csharp
   public class CreateItemCommand : ICommand<CreateItemCommandResponse>, IAuthorizedRequest
   {
       public string Name { get; set; } = string.Empty;
   }
   ```
2. **Response (Must be a class, NOT a record)**:
   ```csharp
   public class CreateItemCommandResponse
   {
       public int Id { get; set; }
       public CreateItemCommandResponse() { }
       public CreateItemCommandResponse(int id) => Id = id;
   }
   ```
3. **Shared Validation Rules (`Features/{Entity}s/Shared/{Entity}ValidationRules.cs`)**:
   ```csharp
   public static class ItemValidationRules
   {
       public static IRuleBuilderOptions<T, string?> ApplyItemNameRules<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
           ruleBuilder.MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters");
   }
   ```
4. **Validator (Calls `ApplyValidationRules()`)**:
   ```csharp
   public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
   {
       public CreateItemCommandValidator()
       {
           ApplyValidationRules();
       }

       private void ApplyValidationRules()
       {
           RuleFor(x => x.Name).Required();
           When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
           {
               RuleFor(x => x.Name).ApplyItemNameRules();
           });
       }
   }
   ```
5. **Handler** (Injects `IDateTimeProvider` for timestamps; checks domain mutation `ErrorOr` result):
   ```csharp
   public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, CreateItemCommandResponse>
   {
       private readonly IUnitOfWork _unitOfWork;
       private readonly IDateTimeProvider _dateTimeProvider;

       public CreateItemCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
       {
           _unitOfWork = unitOfWork;
           _dateTimeProvider = dateTimeProvider;
       }

       public async Task<ErrorOr<CreateItemCommandResponse>> Handle(CreateItemCommand request, CancellationToken ct)
       {
           var entity = new MyEntity(request.Name);
           
           // If performing domain mutation, verify ErrorOr<Success> result
           var updateResult = entity.UpdateStatus(_dateTimeProvider.UtcNow.UtcDateTime);
           if (updateResult.IsError)
               return updateResult.Errors;

           await _unitOfWork.MyEntities.AddAsync(entity, ct);
           await _unitOfWork.SaveChangesAsync(ct);
           return new CreateItemCommandResponse(entity.Id);
       }
   }
   ```

### B. Creating a Query (With AutoMapper Projection)
1. **Query**:
   ```csharp
   public class GetItemByIdQuery : IQuery<GetItemByIdQueryResponse>
   {
       public int Id { get; set; }
   }
   ```
2. **Response (Must be a class, NOT a record)**:
   ```csharp
   public class GetItemByIdQueryResponse
   {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
   }
   ```
3. **AutoMapper Profile**:
   ```csharp
   public class GetItemByIdQueryMappingProfile : Profile
   {
       public GetItemByIdQueryMappingProfile() => CreateMap<MyEntity, GetItemByIdQueryResponse>();
   }
   ```
4. **Handler** (Uses `GetByIdAsync<TDto>` or `GetAsync<TDto>(spec)` with `ProjectTo`):
   ```csharp
   public class GetItemByIdQueryHandler : IQueryHandler<GetItemByIdQuery, GetItemByIdQueryResponse>
   {
       private readonly IUnitOfWork _unitOfWork;
       public GetItemByIdQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

       public async Task<ErrorOr<GetItemByIdQueryResponse>> Handle(GetItemByIdQuery request, CancellationToken ct)
       {
           var response = await _unitOfWork.MyEntities.GetByIdAsync<GetItemByIdQueryResponse>(request.Id, ct);
           if (response is null)
               return Error.NotFound(ErrorCodes.Common.NotFound, "Item not found");

           return response;
       }
   }
   ```

### C. Creating a Paginated List Query (Mandatory for all list/collection queries)
1. **Query (Inherits `PaginatedQuery`)**:
   ```csharp
   public class GetItemsQuery : PaginatedQuery, IQuery<PaginatedResult<GetItemsQueryResponse>>
   {
   }
   ```
2. **Response (Must be a class, NOT a record)**:
   ```csharp
   public class GetItemsQueryResponse
   {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
       public DateTimeOffset CreatedAt { get; set; }
       public DateTimeOffset? UpdatedAt { get; set; }
   }
   ```
3. **AutoMapper Profile**:
   ```csharp
   public class GetItemsQueryMappingProfile : Profile
   {
       public GetItemsQueryMappingProfile() => CreateMap<MyEntity, GetItemsQueryResponse>();
   }
   ```
4. **Specifications**:
   - **Query-Specific Spec** (`Queries/GetItems/Specifications/ItemsPaginatedSpec.cs`):
     ```csharp
     public class ItemsPaginatedSpec : ItemsFilteredSpec
     {
         public ItemsPaginatedSpec(int pageNumber, int pageSize, string? search) : base(search)
         {
             Query.OrderBy(x => x.Name);
             Query.Paginate(pageNumber, pageSize);
             Query.AsNoTracking();
         }
     }
     ```
   - **Common Feature Filter / Count Spec** (`Features/{Entity}s/Specifications/ItemsFilteredSpec.cs`):
     ```csharp
     public class ItemsFilteredSpec : Specification<MyEntity>
     {
         public ItemsFilteredSpec(string? search)
         {
             if (!string.IsNullOrWhiteSpace(search))
                 Query.Where(x => x.Name.Contains(search.Trim()));
         }
     }
     ```
5. **Handler (Fetches paginated items + total count, returns `PaginatedResult`)**:
   ```csharp
   public class GetItemsQueryHandler : IQueryHandler<GetItemsQuery, PaginatedResult<GetItemsQueryResponse>>
   {
       private readonly IUnitOfWork _unitOfWork;
       public GetItemsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

       public async Task<ErrorOr<PaginatedResult<GetItemsQueryResponse>>> Handle(GetItemsQuery request, CancellationToken ct)
       {
           var dataSpec = new ItemsPaginatedSpec(request.PageNumber, request.PageSize, request.Search);
           var countSpec = new ItemsFilteredSpec(request.Search);

           var items = await _unitOfWork.MyEntities.ListAsync<GetItemsQueryResponse>(dataSpec, ct);
           var totalCount = await _unitOfWork.MyEntities.CountAsync(countSpec, ct);

           return new PaginatedResult<GetItemsQueryResponse>(items, totalCount, request.PageNumber, request.PageSize);
       }
   }
   ```
6. **API Controller Action (Accepts `BasicPaginationRequest`)**:
   ```csharp
   [HttpGet]
   public async Task<IActionResult> GetItems([FromQuery] BasicPaginationRequest request, CancellationToken ct)
   {
       var query = _mapper.Map<GetItemsQuery>(request);
       var result = await Mediator.Send(query, ct);

       if (result.IsError)
           return Problem(result.Errors);

       return Ok(result.Value);
   }
   ```

### D. Creating a Modifying API Endpoint
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateMyEntityRequest request, CancellationToken ct)
{
    var command = _mapper.Map<CreateMyEntityCommand>(request);
    var result = await Mediator.Send(command, ct);

    if (result.IsError)
        return Problem(result.Errors);

    return Ok(result.Value);
}
```

For complete technical component patterns, see [patterns-and-components.md](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/patterns-and-components.md).

---

## 6. Verification and Quality Control

Before declaring any task done, run the build verification in the shell from the workspace root:

```powershell
dotnet build Gymify.sln
```

### Quality Checklist:
- [ ] Build succeeded with `0 Error(s)` and `0 Warning(s)`.
- [ ] All list-returning queries inherit `PaginatedQuery` and return `PaginatedResult<T>`.
- [ ] List query controllers accept `[FromQuery] BasicPaginationRequest request`.
- [ ] Command validators use `ApplyValidationRules()` and derive property rules from `{Entity}ValidationRules` in `Shared/`.
- [ ] Query- or command-specific specifications are placed in a `Specifications/` subfolder inside that query/command folder.
- [ ] Only specifications shared across multiple queries/commands within the feature reside in `Features/{Entity}/Specifications/`.
- [ ] No manual entity-to-DTO instantiation in query handlers (`ProjectTo` used).
- [ ] Domain entities have `private set;` on all business properties.
- [ ] Handlers return `ErrorOr<T>` with standard `ErrorCodes`.
- [ ] No `DbContext` directly injected in `Gymify.Application`.
- [ ] Controller action returns `Problem(result.Errors)` or `Ok(result.Value)` (or `CreatedAtRoute` referencing a named `GetById` endpoint).
- [ ] No superfluous route names defined in `RouteNames.cs` (reserved strictly for `GetById` routes used in `CreatedAtRoute`).
- [ ] `_unitOfWork.SaveChangesAsync` is called exactly once per command handler.

---

## 7. Supporting Reference Documents

- [architecture-rules.md](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/architecture-rules.md): In-depth architectural constraints, dependency boundaries, standard tiers, and anti-patterns.
- [patterns-and-components.md](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/patterns-and-components.md): Technical deep-dive on Entities, CQRS, Specifications, AutoMapper, FluentValidation, Caching, and Security.
- [workflows.md](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/workflows.md): Step-by-step workflows for new features, modifications, refactoring, and bug fixes.
- [reference-implementations.md](file:///d:/new%20gym/New%20folder/Gymify/.agents/skills/dotnet-architecture/references/reference-implementations.md): Annotated walkthrough of `Users.UpdateProfile`, `Customers.GetCustomerMe`, and `Primefit/Branches`.
