using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class PackageConfigurations : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("Packages", "gym");

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
        }
    }
}
