using Gymify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymify.Infrastructure.Data.Configurations
{
    public class TrainerKpiScoreConfigurations : IEntityTypeConfiguration<TrainerKpiScore>
    {
        public void Configure(EntityTypeBuilder<TrainerKpiScore> builder)
        {
            builder.ToTable("TrainerKpiScores", "staff");

            builder.Property(s => s.Score)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(s => s.Comment)
                .HasMaxLength(500);

            builder.Property(s => s.Date)
                .IsRequired();

            builder.HasOne(s => s.Trainer)
                .WithMany()
                .HasForeignKey(s => s.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Kpi)
                .WithMany()
                .HasForeignKey(s => s.KpiId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new { s.TrainerId, s.Date });
            builder.HasIndex(s => new { s.TrainerId, s.KpiId, s.Date })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
