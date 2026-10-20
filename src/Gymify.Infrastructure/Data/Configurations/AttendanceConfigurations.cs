using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class AttendanceConfigurations : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances", "gym");

            builder.Property(a => a.Code)
                .HasMaxLength(50);

            builder.Property(a => a.CheckInAt)
                .IsRequired();

            // Filtered index to quickly check if a customer has an active check-in and prevent double check-in
            builder.HasIndex(a => a.CustomerId)
                .IsUnique()
                .HasFilter("[CheckOutAt] IS NULL AND [IsDeleted] = 0");

            // Filtered index to quickly check if a locker is currently occupied and prevent double booking
            builder.HasIndex(a => a.LockerId)
                .IsUnique()
                .HasFilter("[CheckOutAt] IS NULL AND [IsDeleted] = 0");

            // Dedicated filtered index to immediately count active gym attendees without scanning historical rows
            builder.HasIndex(a => a.Id)
                .HasDatabaseName("IX_Attendances_ActiveOccupancy")
                .HasFilter("[CheckOutAt] IS NULL AND [IsDeleted] = 0");

            builder.HasOne(a => a.Customer)
                .WithMany(c => c.Attendances)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Locker)
                .WithMany(l => l.Attendances)
                .HasForeignKey(a => a.LockerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
