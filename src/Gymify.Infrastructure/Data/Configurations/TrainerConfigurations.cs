using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class TrainerConfigurations : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.ToTable("Trainers", "staff");

            builder.HasIndex(t => t.EmployeeId)
                .IsUnique();

            builder.Property(t => t.YearsOfExperience)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(t => t.IsAvailable)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(t => t.Employee)
                .WithOne(e => e.Trainer)
                .HasForeignKey<Trainer>(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
