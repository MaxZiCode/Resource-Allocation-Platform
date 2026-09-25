using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceAllocation.Domain.Models;

namespace ResourceAllocation.Infrastructure.EntityFramework.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasOne(x => x.Type)
               .WithMany(t => t.Resources)
               .HasForeignKey(x => x.TypeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}