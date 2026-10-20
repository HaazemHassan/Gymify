using Gymify.Domain.Entities.Contracts;

namespace Gymify.Domain.Entities.Base
{

    public abstract class FullAuditableEntity<TId> : AuditableEntity<TId>, IFullyAuditableEntity where TId : notnull
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
