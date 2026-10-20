using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", "staff");

            builder.HasIndex(e => e.DomainUserId)
                .IsUnique();

            builder.Property(e => e.Salary)
                .HasPrecision(18, 2);

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(e => e.DomainUser)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.DomainUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
