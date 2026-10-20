using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers", "users");

            builder.HasIndex(c => c.DomainUserId)
                .IsUnique();

            builder.Property(c => c.Code)
                .HasMaxLength(50);

            builder.HasIndex(c => c.Code)
                .IsUnique()
                .HasFilter("[Code] IS NOT NULL AND [IsDeleted] = 0");

            builder.Property(c => c.Injuries)
                .HasMaxLength(1000);

            builder.Property(c => c.ChronicDiseases)
                .HasMaxLength(1000);

            builder.Property(c => c.Notes)
                .HasMaxLength(2000);

            builder.Property(c => c.ApprovalStatus)
                .HasConversion<string>();

            builder.HasOne(c => c.DomainUser)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.DomainUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

