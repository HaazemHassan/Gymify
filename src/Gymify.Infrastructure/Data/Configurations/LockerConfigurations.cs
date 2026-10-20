using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class LockerConfigurations : IEntityTypeConfiguration<Locker>
    {
        public void Configure(EntityTypeBuilder<Locker> builder)
        {
            builder.ToTable("Lockers", "gym");

            builder.Property(l => l.Number)
                .IsRequired();

            builder.HasIndex(l => l.Number)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(l => l.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
