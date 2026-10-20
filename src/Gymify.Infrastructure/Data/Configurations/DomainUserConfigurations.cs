using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymify.Domain.Entities;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class DomainUserConfigurations : IEntityTypeConfiguration<DomainUser>
    {
        public void Configure(EntityTypeBuilder<DomainUser> builder)
        {
            builder.ToTable("DomainUsers", "users");
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(u => u.Customer)
                .WithOne(c => c.DomainUser)
                .HasForeignKey<Customer>(c => c.DomainUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
