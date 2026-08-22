using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Irrigation.Infrastructure.Outbox
{
    public class OutboxConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("Outbox");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.Data)
                .IsRequired();

            builder.Property(x => x.Error);

            builder.HasIndex(x => x.ProcessedAt);

            builder.Ignore(x => x.Processed);
        }
    }
}
