using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class SubscriptionConfigurations : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions", "gym", t =>
            {
                t.HasCheckConstraint(
                    "CK_Subscriptions_Type_TrainerId",
                    "([Type] = 'Private' AND [TrainerId] IS NOT NULL) OR ([Type] <> 'Private' AND [TrainerId] IS NULL)");
            });

            builder.HasOne(s => s.Customer)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Package)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(s => s.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Trainer)
                .WithMany()
                .HasForeignKey(s => s.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(s => s.PackageName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.PackagePrice)
                .HasPrecision(18, 2);

            builder.Property(s => s.PackageDuration)
                .IsRequired();

            builder.Property(s => s.PackageDescription)
                .HasMaxLength(1000);

            builder.Property(s => s.StartDate)
                .IsRequired();

            builder.Property(s => s.EndDate)
                .IsRequired();

            builder.Property(s => s.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(s => s.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue(SubscriptionType.Regular);

            builder.Property(s => s.Notes)
                .HasMaxLength(2000);

            builder.Property(s => s.IsCancelled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(s => s.CancellationReason)
                .HasMaxLength(500);

            builder.HasMany(s => s.Freezes)
                .WithOne(f => f.Subscription)
                .HasForeignKey(f => f.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new { s.CustomerId, s.EndDate });
            builder.HasIndex(s => s.IsCancelled);
        }
    }
}
