using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceAllocation.Domain.Models;

namespace ResourceAllocation.Infrastructure.EntityFramework.Configurations;

public class ResourceTypeConfiguration : IEntityTypeConfiguration<ResourceType>
{
    public void Configure(EntityTypeBuilder<ResourceType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();
    }
}