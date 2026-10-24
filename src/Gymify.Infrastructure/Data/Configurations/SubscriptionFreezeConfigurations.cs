using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class SubscriptionFreezeConfigurations : IEntityTypeConfiguration<SubscriptionFreeze>
    {
        public void Configure(EntityTypeBuilder<SubscriptionFreeze> builder)
        {
            builder.ToTable("SubscriptionFreezes", "gym");

            builder.Property(f => f.StartDate)
                .IsRequired();

            builder.Property(f => f.EndDate)
                .IsRequired();

            builder.Property(f => f.Reason)
                .HasMaxLength(500);

            builder.HasOne(f => f.Subscription)
                .WithMany(s => s.Freezes)
                .HasForeignKey(f => f.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => new { f.SubscriptionId, f.StartDate, f.EndDate });
        }
    }
}
