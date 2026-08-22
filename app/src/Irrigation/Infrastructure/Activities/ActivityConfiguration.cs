using Irrigation.Domain.Activities;
using Irrigation.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Irrigation.Infrastructure.Activities;

public sealed class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ActivityId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion(
                type => type.Value,
                value => new ActivityType(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Category)
            .HasConversion(
                category => category.Value,
                value => new ActivityCategory(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Origin)
            .HasConversion(
                origin => origin.ToString(),
                value => ActionOrigin.From(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Data)
            .IsRequired();

        builder.OwnsOne(x => x.Subject, subject =>
        {
            subject.Property(x => x.Type)
                .HasMaxLength(100)
                .IsRequired();

            subject.Property(x => x.Id)
                .IsRequired();
        });

        builder.HasIndex(x => x.Timestamp);

        builder.Ignore(x => x.Events);
    }
}
