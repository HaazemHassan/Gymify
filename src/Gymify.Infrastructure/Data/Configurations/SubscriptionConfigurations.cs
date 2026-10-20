using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class SubscriptionConfigurations : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions", "gym");

            builder.HasOne(s => s.Customer)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Package)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(s => s.PackageId)
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

            builder.Property(s => s.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(s => s.Notes)
                .HasMaxLength(2000);
        }
    }
}
