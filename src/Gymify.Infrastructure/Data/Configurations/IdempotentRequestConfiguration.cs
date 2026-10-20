using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymify.Infrastructure.Idempotency;

namespace Gymify.Infrastructure.Data.Configurations
{
    internal sealed class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
    {
        public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
        {
            builder.ToTable("IdempotentRequests", "idempotency");

            builder.HasKey(ir => ir.Id);

            builder.Property(ir => ir.Name).IsRequired();

            builder.Property(ir => ir.Response).IsRequired(false);

            builder.Property(ir => ir.ResponseType)
                   .HasMaxLength(500)
                   .IsRequired(false);
        }
    }
}
