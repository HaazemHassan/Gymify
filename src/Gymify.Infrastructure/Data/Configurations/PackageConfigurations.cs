using Gymify.Domain.Common.Enums;
using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class PackageConfigurations : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("Packages", "gym", t =>
            {
                t.HasCheckConstraint(
                    "CK_Packages_Type_TrainerId",
                    "([Type] = 'Private' AND [TrainerId] IS NOT NULL) OR ([Type] <> 'Private' AND [TrainerId] IS NULL)");
            });

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.Duration)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue(PackageType.Regular);

            builder.HasOne(p => p.Trainer)
                .WithMany()
                .HasForeignKey(p => p.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
