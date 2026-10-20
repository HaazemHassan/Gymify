namespace Gymify.Domain.Entities.Contracts {
    public interface ISoftDeletableEntity {
        bool IsDeleted { get; set; }
        DateTimeOffset? DeletedAt { get; set; }
        int? DeletedBy { get; set; }
    }
}
