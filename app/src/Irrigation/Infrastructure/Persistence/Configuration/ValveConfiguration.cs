using Irrigation.Domain.Devices;
using Irrigation.Domain.Shared;
using Irrigation.Domain.Valves;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Irrigation.Infrastructure.Persistence.Configuration;

public sealed class ValveConfiguration : IEntityTypeConfiguration<Valve>
{
    public void Configure(EntityTypeBuilder<Valve> builder)
    {
        builder.ToTable("Valves");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ValveId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.DeviceId)
            .HasConversion(
                id => id.Value,
                value => new DeviceId(value))
            .IsRequired();

        builder.Property(x => x.Index)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder
            .HasOne<Device>()
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
            {
                x.DeviceId, HardwareId = x.Index
            })
            .IsUnique();

        builder.Ignore(x => x.Events);
    }
}