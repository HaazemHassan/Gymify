using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class CustomerBehaviorNoteConfigurations : IEntityTypeConfiguration<CustomerBehaviorNote>
    {
        public void Configure(EntityTypeBuilder<CustomerBehaviorNote> builder)
        {
            builder.ToTable("CustomerBehaviorNotes", "gym");

            builder.Property(n => n.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.HasOne(n => n.Customer)
                .WithMany(c => c.CustomerBehaviorNotes)
                .HasForeignKey(n => n.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Employee)
                .WithMany(e => e.CustomerBehaviorNotes)
                .HasForeignKey(n => n.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(n => new { n.CustomerId, n.CreatedAt });
            builder.HasIndex(n => new { n.EmployeeId, n.CreatedAt });
        }
    }
}
