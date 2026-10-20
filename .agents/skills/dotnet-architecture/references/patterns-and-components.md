# Core Patterns & Component Reference

This document provides concrete technical patterns, code standards, and component examples verified directly against the active codebase.

---

## 1. Domain Entities & Invariant Enforcement

### Entity Base Classes
Entities reside in `Gymify.Domain.Entities` and inherit from one of the base classes in `Gymify.Domain.Entities.Base`:
- `FullAuditableEntity<TKey>`: Id, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, IsDeleted, DeletedAt, DeletedBy (Soft delete).
- `AuditableEntity<TKey>`: Id, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy.
- `BaseEntity<TKey>`: Id, DomainEvents collection, equality operators.

### Entity Design Standards
1. **Private parameterless constructor**: Required for EF Core materialization.
2. **Explicit parameterized constructor or static factory method `Create(...)`**:
   - Return `ErrorOr<TEntity>` if invariant validation can fail upon creation.
   - Return instance directly if parameters are primitive-validated before calling constructor.
3. **Private property setters**: Properties must have `private set;`.
4. **Explicit business mutation methods**: Mutate state through meaningful domain methods (`UpdatePhysicalAttributes(...)`, `UpdateHealthInfo(...)`, `UpdateInfo(...)`).
5. **Encapsulated collections**: Initialize with `new List<T>()` or private backing field `_items`. Expose as `ICollection<T>` or `IReadOnlyCollection<T>`.

### Verified Code Example (from `src/Gymify.Domain/Entities/Customer.cs`)
```csharp
using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities.Base;

namespace Gymify.Domain.Entities
{
    public sealed class Customer : FullAuditableEntity<int>
    {
        private Customer() { } // EF Core requirement

        public Customer(
            int domainUserId,
            double? height = default,
            double? weight = default,
            ApprovalStatus approvalStatus = ApprovalStatus.Pending)
        {
            DomainUserId = domainUserId;
            Height = height;
            Weight = weight;
            ApprovalStatus = approvalStatus;
        }

        public int DomainUserId { get; private set; }
        public DomainUser DomainUser { get; private set; } = null!;

        public double? Height { get; private set; }
        public double? Weight { get; private set; }
        public ApprovalStatus ApprovalStatus { get; private set; } = ApprovalStatus.Pending;

        public ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

        public void UpdatePhysicalAttributes(double? height, double? weight)
        {
            if (height.HasValue) Height = height.Value;
            if (weight.HasValue) Weight = weight.Value;
        }

        public void UpdateApprovalStatus(ApprovalStatus status)
        {
            ApprovalStatus = status;
        }
    }
}
```

---

## 2. CQRS Abstractions & MediatR Pipeline

### Interfaces (`src/Gymify.Application/Common/CQRS/`)
- `ICommand`: Represents a void command returning `ErrorOr<Success>`.
- `ICommand<TResponse>`: Represents a command returning `ErrorOr<TResponse>`.
- `ICommandHandler<TCommand>`: Implements `IRequestHandler<TCommand, ErrorOr<Success>>`.
- `ICommandHandler<TCommand, TResponse>`: Implements `IRequestHandler<TCommand, ErrorOr<TResponse>>`.
- `IQuery<TResponse>`: Represents a query returning `ErrorOr<TResponse>`.
- `IQueryHandler<TQuery, TResponse>`: Implements `IRequestHandler<TQuery, ErrorOr<TResponse>>`.

### Strict CQRS Naming & Class Structure Rules
To maintain consistency and enable automated discovery:
1. **Name Alignment**: Every artifact in a CQRS feature folder must strictly reflect the request name:
   - Command: `{ActionName}Command`, `{ActionName}CommandHandler`, `{ActionName}CommandResponse`, `{ActionName}CommandValidator`.
   - Query: `{QueryName}Query`, `{QueryName}QueryHandler`, `{QueryName}QueryResponse`, `{QueryName}QueryMappingProfile`.
   - *Prohibited*: Divergent names like `PackageManagementResponse` for `GetPackagesForManagementQuery`. It must always be `GetPackagesForManagementQueryResponse`.
2. **Responses Must Be Classes (Never Records)**:
   - Always declare responses as standard classes:
     ```csharp
     public class GetPackageByIdQueryResponse
     {
         public int Id { get; set; }
         public string Name { get; set; } = string.Empty;
         public decimal Price { get; set; }
     }
     ```
   - **Do NOT use `record` for response types**. AutoMapper `ProjectTo<T>` projections generate SQL expressions that require parameterless constructors and mutable properties; positional records cause projection binding failures and mapping overhead.

### MediatR Pipeline Behaviors (Executed in exact order)
Configured in `src/Gymify.Application/ApplicationServiceRegistration.cs`:
1. `TrimmingBehavior<,>`: Trims whitespace from all string properties on inbound requests.
2. `LoggingBehavior<,>`: Logs request name, execution duration, and errors.
3. `AuthorizationBehavior<,>`: Evaluates `[Authorize]` attributes and enforcers for `IAuthorizedRequest` and `IOwnedResourceRequest`.
4. `ValidationBehavior<,>`: Runs all registered FluentValidation `AbstractValidator<T>` instances; short-circuits with `Error.Validation` if failures occur.
5. `IdempotentCommandPipelineBehavior<,>`: Ensures idempotent command processing when idempotency tokens are supplied.
6. `CachingBehavior<,>`: Checks `ICachePolicy<T>` and returns cached data if available.
7. `CacheInvalidationBehavior<,>`: Invalidates cache tags defined by `ICacheInvalidationPolicy<T>` upon successful command execution.
8. `PostCommitBehavior<,>`: Executes post-commit actions after transactions complete.
9. `TransactionBehavior<,>`: Wraps modifying commands in a database transaction.

---

## 3. Repositories & Unit of Work

### Repository Abstraction (`IGenericRepository<T>`)
Inherits from Ardalis `IRepositoryBase<T>`. Provides projection methods that use AutoMapper `ProjectTo`:

```csharp
// Fetch single entity projected directly to DTO (no over-fetching)
Task<TDestination?> GetByIdAsync<TDestination>(int id, CancellationToken ct);
Task<TDestination?> GetAsync<TDestination>(ISpecification<T> spec, CancellationToken ct);
Task<TDestination?> GetAsync<TDestination>(Expression<Func<T, bool>> predicate, CancellationToken ct);

// Fetch lists projected directly to DTOs
Task<List<TDestination>> ListAsync<TDestination>(ISpecification<T> spec, CancellationToken ct);
Task<List<TDestination>> ListAsync<TDestination>(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

// Query helpers
Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
Task<int> CountAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct);
```

### Unit of Work (`IUnitOfWork`)
Exposes strongly typed repositories and transaction control:
```csharp
public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ICustomerRepository Customers { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IVerificationCodeRepository VerificationCodes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    bool HasCurrentTransaction();
    IDatabaseExecutionStrategy CreateExecutionStrategy();
}
```

---

## 4. Ardalis Specification Pattern

### Why and When to Use
- **Use when**: Querying entities with criteria, relations (`Include`), sorting, or pagination.
- **Do not use when**: Doing a simple ID lookup where `_unitOfWork.Repo.GetByIdAsync(id)` or `GetByIdSpec<T>` suffices.

### Specification Colocation Hierarchy
To keep the codebase maintainable and strictly scoped:
1. **Query- or Command-Specific Specifications**:
   - Must be placed in a `Specifications/` subfolder located *inside* the respective query or command folder.
   - Example: `Features/Customers/Queries/GetCustomerMe/Specifications/CustomerByDomainUserIdSpec.cs`
   - Example: `Features/Packages/Queries/GetPackagesForCustomers/Specifications/ActivePackagesPaginatedSpec.cs`
2. **Common Feature Specifications**:
   - If a specification is shared across multiple queries or commands within the feature, place it in `Features/{Entity}s/Specifications/`.
   - Example: `Features/Packages/Specifications/PackagesFilteredSpec.cs` (shared filter used by both customer and management list queries for counting).
3. **Global Shared Specifications**:
   - Cross-aggregate generic specifications belong in `Gymify.Application/Specifications/Shared/` (e.g., `GetByIdSpec<T>`).

### Verified Code Example (from `src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/Specifications/CustomerByDomainUserIdSpec.cs`)
```csharp
using Ardalis.Specification;
using Gymify.Domain.Entities;

namespace Gymify.Application.Features.Customers.Queries.GetCustomerMe.Specifications
{
    public class CustomerByDomainUserIdSpec : Specification<Customer>
    {
        public CustomerByDomainUserIdSpec(int domainUserId)
        {
            Query
                .Where(c => c.DomainUserId == domainUserId)
                .Include(c => c.DomainUser);
        }
    }
}
```

---

## 5. AutoMapper Projections vs In-Memory Mapping

### Query Projections (Database Level)
In Query handlers, do **not** load the domain entity into memory to map it manually. AutoMapper configuration enables EF Core to translate projections into SQL `SELECT` statements containing only requested fields.

```csharp
// Profile in Query folder (e.g., CustomerMeMappingProfile.cs)
public class CustomerMeMappingProfile : Profile
{
    public CustomerMeMappingProfile()
    {
        CreateMap<Customer, CustomerMeResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DomainUser != null ? src.DomainUser.Name : string.Empty));
    }
}

// Handler execution leverages ProjectTo internally:
var response = await _unitOfWork.Customers.GetAsync<CustomerMeResponse>(spec, cancellationToken);
```

### In-Memory Command Mapping
Commands modify entities in memory, so AutoMapper is used in-memory after persisting:
```csharp
await _unitOfWork.SaveChangesAsync(cancellationToken);
var response = _mapper.Map<DomainUser, UpdateProfileCommandResponse>(userFromDb);
return response;
```

---

## 6. Pagination Architecture (Mandatory for All Collection/List Queries)

Every query returning a collection or list must be paginated to ensure optimal database performance, predictable response payloads, and consistent client integration.

### Core Pagination Types (`src/Gymify.Application/Common/Pagination/`)
- **`PaginatedQuery`**: Base class for queries. Exposes:
  - `int PageNumber { get; set; } = 1;`
  - `int PageSize { get; set; } = 10;`
  - `string? Search { get; set; }`
- **`PaginatedResult<T>`**: Standard generic container returned by paginated handlers:
  - `List<T> Data { get; set; }`
  - `int CurrentPage { get; set; }`
  - `int TotalPages { get; set; }`
  - `int TotalCount { get; set; }`
  - `int PageSize { get; set; }`
  - `bool HasPreviousPage => CurrentPage > 1;`
  - `bool HasNextPage => CurrentPage < TotalPages;`
  - `object? Meta { get; set; }` (optional statistics or metadata)

### Ardalis Specification Pagination (`SpecificationBuilderExtensions.cs`)
Use the extension method `Query.Paginate(pageNumber, pageSize)` which automatically applies `.Skip(...).Take(...)` with boundary safeguards against invalid page numbers or page sizes.

```csharp
// 1. Filtered Specification (used for CountAsync and as base for pagination)
public class ItemsFilteredSpec : Specification<MyEntity>
{
    public ItemsFilteredSpec(string? search = null, bool? isActive = null)
    {
        if (isActive.HasValue)
            Query.Where(x => x.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(search))
            Query.Where(x => x.Name.ToLower().Contains(search.Trim().ToLower()));
    }
}

// 2. Paginated Specification (inherits filter spec, applies ordering and pagination)
public class ItemsPaginatedSpec : ItemsFilteredSpec
{
    public ItemsPaginatedSpec(int pageNumber, int pageSize, string? search = null, bool? isActive = null)
        : base(search, isActive)
    {
        Query.OrderBy(x => x.CreatedAt);
        Query.Paginate(pageNumber, pageSize);
        Query.AsNoTracking();
    }
}
```

### Handler Pattern
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

### API Layer Integration (`BasicPaginationRequest`)
Controllers accept `[FromQuery] BasicPaginationRequest request`:
```csharp
// src/Gymify.API/Requests/Common/Pagination/BasicPaginationRequest.cs
public class BasicPaginationRequest
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; }
}

// Mapping profile preserves defaults if query string parameters are omitted:
public class BasicPaginationRequestMappingProfile : Profile
{
    public BasicPaginationRequestMappingProfile()
    {
        CreateMap<BasicPaginationRequest, GetItemsQuery>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember is not null));
    }
}

// Controller action:
[HttpGet]
public async Task<IActionResult> GetItems([FromQuery] BasicPaginationRequest request, CancellationToken ct)
{
    var query = _mapper.Map<GetItemsQuery>(request);
    var result = await Mediator.Send(query, ct);
    if (result.IsError) return Problem(result.Errors);
    return Ok(result.Value);
}
```

---

## 7. Validation (FluentValidation)

Command validation follows the **Single Source of Truth** pattern to prevent validation drift across different commands updating the same entity:

### 1. Centralized Rules Class (`Features/{Entity}s/Shared/{Entity}ValidationRules.cs`)
Define static extension methods on `IRuleBuilder` for each domain property:
```csharp
namespace Gymify.Application.Features.Packages.Shared
{
    public static class PackageValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ApplyPackageNameRules<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
            ruleBuilder.MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters");

        public static IRuleBuilderOptions<T, decimal> ApplyPriceRules<T>(this IRuleBuilder<T, decimal> ruleBuilder) =>
            ruleBuilder.GreaterThan(0).WithMessage("Price must be greater than 0");

        public static IRuleBuilderOptions<T, int> ApplyDurationRules<T>(this IRuleBuilder<T, int> ruleBuilder) =>
            ruleBuilder.GreaterThan(0).WithMessage("Duration must be greater than 0");
    }
}
```
Global primitive rules (like `.Required()`, `.ApplyEmailRules()`, `.ApplyPhoneNumberRules()`, `.ApplyAddressRules()`) reside in `Gymify.Application.Common.CommonValidationRules`.

### 2. Command Validator Pattern (Invoking `ApplyValidationRules()`)
Command validators inherit `AbstractValidator<TCommand>` and structure their rules inside an explicit `ApplyValidationRules()` method called from the constructor:
```csharp
public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
{
    public CreatePackageCommandValidator()
    {
        ApplyValidationRules();
    }

    private void ApplyValidationRules()
    {
        RuleFor(x => x.Name).Required();
        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name).ApplyPackageNameRules();
        });

        RuleFor(x => x.Price).ApplyPriceRules();
        RuleFor(x => x.Duration).ApplyDurationRules();
    }
}
```

---

## 8. Security & Authorization

### Security Attributes & Markers
- `[Authorize]`: Requires authenticated user.
- `[Authorize(UserTypes = [UserType.PartnerAdmin])]`: Restricts by user type.
- `[Authorize(Policy = AuthorizationPolicies.SelfOnly)]`: Enforces that the requesting user owns the targeted resource.
- `IAuthorizedRequest`: Marker indicating request requires authorization checks.
- `IOwnedResourceRequest`: Enforces resource ownership; must expose `int UserId { get; }`.

```csharp
[Authorize(Policy = AuthorizationPolicies.SelfOnly)]
public class UpdateProfileCommand : ICommand<UpdateProfileCommandResponse>, IOwnedResourceRequest
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
}
```

---

## 9. Caching & Cache Invalidation

### Query Caching Policy (`ICachePolicy<TQuery>`)
```csharp
public class GetEntityByIdQueryCachePolicy : ICachePolicy<GetEntityByIdQuery>
{
    public TimeSpan? Expiration => TimeSpan.FromMinutes(15);
    public string GetCacheKey(GetEntityByIdQuery request) => $"entities:{request.Id}";
    public string[] GetCacheTags(GetEntityByIdQuery request) => [$"entities:{request.Id}", "entities:list"];
    public bool ShouldSkipCache(GetEntityByIdQuery request) => false;
}
```

### Command Cache Invalidation Policy (`ICacheInvalidationPolicy<TCommand>`)
```csharp
public class UpdateEntityCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<UpdateEntityCommand>
{
    public IEnumerable<string> GetTags(UpdateEntityCommand request)
    {
        yield return $"entities:{request.Id}";
        yield return "entities:list";
    }
}
```

---

## 10. API Controllers & Error Mapping

### Controller Conventions
- Controllers inherit `BaseController`.
- Injects `IMediator` (available as protected property) and `IMapper` (if mapping Request -> Command/Query).
- Map error types automatically to RFC 7807 `ProblemDetails`:
  - `ErrorType.Conflict` ➔ `409 Conflict`
  - `ErrorType.Validation` ➔ `400 BadRequest`
  - `ErrorType.NotFound` ➔ `404 NotFound`
  - `ErrorType.Unauthorized` ➔ `401 Unauthorized`
  - `ErrorType.Forbidden` ➔ `403 Forbidden`
  - Default ➔ `500 InternalServerError`

```csharp
[HttpPatch("me")]
[Authorize]
public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
{
    var command = _mapper.Map<UpdateProfileCommand>(request);
    command.UserId = _currentUserService.UserId!.Value;

    var result = await Mediator.Send(command);
    if (result.IsError)
    {
        return Problem(result.Errors);
    }

    return Ok(result.Value);
}
```
