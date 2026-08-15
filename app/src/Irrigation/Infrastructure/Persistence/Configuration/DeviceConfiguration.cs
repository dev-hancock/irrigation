using Irrigation.Domain.Devices;
using Irrigation.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Irrigation.Infrastructure.Persistence.Configuration;

public sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new DeviceId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.HardwareId)
            .HasConversion(
                id => id.Value,
                value => new HardwareId(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Firmware)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Model)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Version)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasIndex(x => x.HardwareId)
            .IsUnique();

        builder.Ignore(x => x.Events);
    }
}