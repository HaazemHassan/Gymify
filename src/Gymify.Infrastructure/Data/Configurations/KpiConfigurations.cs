using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class KpiConfigurations : IEntityTypeConfiguration<Kpi>
    {
        public void Configure(EntityTypeBuilder<Kpi> builder)
        {
            builder.ToTable("Kpis", "staff");

            builder.Property(k => k.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(k => k.Description)
                .HasMaxLength(500);

            builder.Property(k => k.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(k => k.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
