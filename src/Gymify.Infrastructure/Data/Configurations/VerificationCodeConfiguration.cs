using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gymify.Domain.Entities;

namespace Gymify.Infrastructure.Data.Configurations
{
    internal class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
    {
        public void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            builder.ToTable("VerificationCodes", "auth");
        }
    }
}
