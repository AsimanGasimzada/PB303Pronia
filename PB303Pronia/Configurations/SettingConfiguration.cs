using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PB303Pronia.Models;

namespace PB303Pronia.Configurations;

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.HasIndex(x=>x.Key).IsUnique();

        builder.Property(x => x.Value).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Key).IsRequired().HasMaxLength(256);
    }
}
