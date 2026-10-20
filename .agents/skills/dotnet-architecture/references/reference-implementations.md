# Reference Implementations

This document details the exact reference features in the repository that future agents must consult when implementing or modifying features.

---

## 1. Primary Command Reference: `Users.UpdateProfile`

### Location
- **API Request**: [UpdateProfileRequest.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Requests/Users/UpdateProfile/UpdateProfileRequest.cs)
- **API Mapping Profile**: [UpdateProfileRequestMappingProfile.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Requests/Users/UpdateProfile/UpdateProfileRequestMappingProfile.cs)
- **Command Definition**: [UpdateProfileCommand.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommand.cs)
- **Command Validator**: [UpdateProfileCommandValidator.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommandValidator.cs)
- **Command Handler**: [UpdateProfileCommandHandler.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommandHandler.cs)
- **Command Response**: [UpdateProfileCommandResponse.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Users/Commands/UpdateProfile/UpdateProfileCommandResponse.cs)
- **Controller Action**: [UsersController.cs (lines 50-65)](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Controllers/UsersController.cs#L50-L65)

### Patterns Demonstrated
1. **Authorization & Security**:
   - Implements `IOwnedResourceRequest` marker interface exposing `int UserId { get; set; }`.
   - Annotated with `[Authorize(Policy = AuthorizationPolicies.SelfOnly)]` which ensures users can only update their own profile.
2. **FluentValidation**:
   - Combines conditional rules (`When(...)`) with shared extension methods (`ApplyUserNameRules()`, `ApplyPhoneNumberRules()`).
   - Uses custom rule `.Must(HaveAtLeastOneNonNullProperty)` to prevent empty updates.
3. **Repository & Unit of Work Usage**:
   - Retrieves tracked entity via `_unitOfWork.Users.GetByIdAsync(...)`.
   - Verifies conflict/uniqueness using `_unitOfWork.Users.AnyAsync(...)`.
   - Mutates entity via domain method `userFromDb.UpdateInfo(...)`.
   - Persists atomically with a single `await _unitOfWork.SaveChangesAsync(cancellationToken)`.
4. **Error Handling (`ErrorOr`)**:
   - Returns `Error.NotFound(description: "User not found")`.
   - Returns `Error.Conflict(code: ErrorCodes.User.PhoneAlreadyExists, description: "Phone number already exists")`.
5. **API Transport**:
   - `UpdateProfileRequestMappingProfile` maps HTTP request DTO to the MediatR command.
   - Controller assigns the authenticated user ID from `ICurrentUserService` and delegates to `Mediator.Send(...)`.

---

## 2. Primary Query Reference: `Customers.GetCustomerMe`

### Location
- **Query Definition**: [GetCustomerMeQuery.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/GetCustomerMeQuery.cs)
- **Specification**: [CustomerByDomainUserIdSpec.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/CustomerByDomainUserIdSpec.cs)
- **AutoMapper Profile**: [CustomerMeMappingProfile.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/CustomerMeMappingProfile.cs)
- **Query Response**: [CustomerMeResponse.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/CustomerMeResponse.cs)
- **Query Handler**: [GetCustomerMeQueryHandler.cs](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.Application/Features/Customers/Queries/GetCustomerMe/GetCustomerMeQueryHandler.cs)
- **Controller Action**: [CustomersController.cs (lines 10-21)](file:///d:/new%20gym/New%20folder/Gymify/src/Gymify.API/Controllers/CustomersController.cs#L10-L21)

### Patterns Demonstrated
1. **Clean CQRS Query**:
   - `GetCustomerMeQuery` implements `IQuery<CustomerMeResponse>`.
2. **Ardalis Specification**:
   - Encapsulates domain user matching and navigation include (`Query.Where(...).Include(c => c.DomainUser)`).
3. **AutoMapper Projection (No Manual Mapping)**:
   - `CustomerMeMappingProfile` defines mappings from `Customer` to `CustomerMeResponse`.
   - Handler calls:
     ```csharp
     var customerResponse = await _unitOfWork.Customers.GetAsync<CustomerMeResponse>(spec, cancellationToken);
     ```
   - EF Core translates this directly into a database-level projected SQL query via `ProjectTo`, fetching only needed columns.
4. **Error Handling**:
   - Returns `Error.NotFound(code: ErrorCodes.Customer.CustomerNotFound, description: "Customer profile not found")`.
5. **Controller Simplicity**:
   - Controller has zero business logic; it dispatches query and returns `Problem(errors)` or `Ok(value)`.

---

## 3. Primary Paginated List Query Reference: `Packages.GetPackagesForCustomers`

This reference establishes the permanent pattern for any query retrieving a collection/list of items.

### Key Architectural Characteristics
1. **Query Definition**:
   - Inherits `PaginatedQuery` (`PageNumber`, `PageSize`, `Search`).
   - Implements `IQuery<PaginatedResult<GetPackagesForCustomersQueryResponse>>`.
   - Protects with `[Authorize(Permissions = [...])]` and `IAuthorizedRequest` when restricted (as in `GetPackagesForManagementQuery`).
2. **Dual Specifications & Colocation Hierarchy**:
   - Query-Specific Paginated Specification: `ActivePackagesPaginatedSpec` located in `Queries/GetPackagesForCustomers/Specifications/ActivePackagesPaginatedSpec.cs`. Inherits the filter spec, applies ordering (`OrderBy(p => p.Price)`), pagination (`Query.Paginate(pageNumber, pageSize)`), and `Query.AsNoTracking()`.
   - Feature-Common Filter Specification: `PackagesFilteredSpec` located in `Features/Packages/Specifications/PackagesFilteredSpec.cs`. Encapsulates `Where` criteria (active flag, search keyword) and is reused across queries for counting.
3. **Shared Validation Rules (Single Source of Truth)**:
   - Feature property validation constraints reside in `Features/Packages/Shared/PackageValidationRules.cs`.
   - Used by `CreatePackageCommandValidator` and `UpdatePackageCommandValidator` via `ApplyValidationRules()`.
4. **Handler Execution**:
   - Executes database projection: `await _unitOfWork.Packages.ListAsync<GetPackagesForCustomersQueryResponse>(dataSpec, ct)`.
   - Executes count: `await _unitOfWork.Packages.CountAsync(countSpec, ct)`.
   - Returns `new PaginatedResult<TResponse>(items, totalCount, request.PageNumber, request.PageSize)`.
5. **API Binding**:
   - Controller accepts `[FromQuery] BasicPaginationRequest request`.
   - AutoMapper maps `BasicPaginationRequest` to the query, preserving defaults when query string properties are null.

---

## 4. Companion Advanced Reference: Caching & Domain Events (Self-Contained)

The active `src/Gymify` template includes the full caching pipeline (`CachingBehavior`, `CacheInvalidationBehavior`) and MediatR event handling in `ApplicationServiceRegistration.cs`. Use these complete, self-contained reference implementations whenever adding caching or domain events to any feature:

### A. Query Caching Policy Pattern (`ICachePolicy<TQuery>`)
Place in `Features/{Entity}s/Queries/{QueryName}/{QueryName}QueryCachePolicy.cs`:
```csharp
using Gymify.Application.Common.Caching;

namespace Gymify.Application.Features.SampleEntities.Queries.GetSampleEntityById
{
    public class GetSampleEntityByIdQueryCachePolicy : ICachePolicy<GetSampleEntityByIdQuery>
    {
        // Cache TTL
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);

        // Unique cache key for this specific query instance
        public string GetCacheKey(GetSampleEntityByIdQuery request) =>
            $"sample-entities:id:{request.Id}";

        // Tags used for grouped cache invalidation
        public string[] GetCacheTags(GetSampleEntityByIdQuery request) =>
        [
            $"sample-entities:id:{request.Id}",
            "sample-entities:list"
        ];

        // Conditional skip logic (e.g., skip cache for certain users or flags)
        public bool ShouldSkipCache(GetSampleEntityByIdQuery request) => false;
    }
}
```

### B. Command Cache Invalidation Policy Pattern (`ICacheInvalidationPolicy<TCommand>`)
Place in `Features/{Entity}s/Commands/{ActionName}/{ActionName}CommandCacheInvalidationPolicy.cs`:
```csharp
using Gymify.Application.Common.Caching;

namespace Gymify.Application.Features.SampleEntities.Commands.CreateSampleEntity
{
    public class CreateSampleEntityCommandCacheInvalidationPolicy : ICacheInvalidationPolicy<CreateSampleEntityCommand>
    {
        // Yield tags that must be evicted when this command succeeds
        public IEnumerable<string> GetTags(CreateSampleEntityCommand request)
        {
            yield return "sample-entities:list";
        }
    }
}
```

### C. Domain Events Pattern (`IDomainEvent`)
When an aggregate root triggers a side effect:

1. **Event Definition** (`Gymify.Domain/DomainEvents/SampleEntityUpdatedDomainEvent.cs`):
```csharp
using Gymify.Domain.Primitives;

namespace Gymify.Domain.DomainEvents
{
    public record SampleEntityUpdatedDomainEvent(int EntityId, string Name) : IDomainEvent;
}
```

2. **Raising the Event in the Aggregate Root** (`Gymify.Domain/Entities/SampleEntity.cs`):
```csharp
public void UpdateName(string newName)
{
    Name = newName;
    AddDomainEvent(new SampleEntityUpdatedDomainEvent(Id, newName));
}
```

3. **Event Handler in Application Layer** (`Gymify.Application/Features/SampleEntities/Events/SampleEntityUpdatedDomainEventHandler.cs`):
```csharp
using MediatR;
using Microsoft.Extensions.Logging;
using Gymify.Domain.DomainEvents;

namespace Gymify.Application.Features.SampleEntities.Events
{
    public class SampleEntityUpdatedDomainEventHandler : INotificationHandler<SampleEntityUpdatedDomainEvent>
    {
        private readonly ILogger<SampleEntityUpdatedDomainEventHandler> _logger;

        public SampleEntityUpdatedDomainEventHandler(ILogger<SampleEntityUpdatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SampleEntityUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain event handled: Entity {Id} updated to {Name}", notification.EntityId, notification.Name);
            return Task.CompletedTask;
        }
    }
}
```

