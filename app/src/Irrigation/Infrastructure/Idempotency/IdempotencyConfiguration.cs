using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Irrigation.Infrastructure.Idempotency
{
    public class IdempotencyConfiguration : IEntityTypeConfiguration<IdempotentMessage>
    {
        public void Configure(EntityTypeBuilder<IdempotentMessage> builder)
        {
            builder.ToTable("Idempotency");

            builder.HasKey(x => new
            {
                x.MessageId,
                x.Handler
            });

            builder.Property(x => x.Handler)
                .HasMaxLength(512)
                .IsRequired();

            builder.HasIndex(x => x.ProcessedAt);
        }
    }
}
