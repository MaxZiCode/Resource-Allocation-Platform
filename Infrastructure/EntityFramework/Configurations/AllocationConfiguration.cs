using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceAllocation.Domain.Models;

namespace ResourceAllocation.Infrastructure.EntityFramework.Configurations;

public class AllocationConfiguration : IEntityTypeConfiguration<Allocation>
{
    public void Configure(EntityTypeBuilder<Allocation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Period)
               .HasColumnType("tsrange")
               .IsRequired();

        builder.Property(x => x.Status)
               .IsRequired();

        builder.Property(x => x.CreatedAt)
               .IsRequired();

        builder.HasOne(x => x.Resource)
               .WithMany(r => r.Allocations)
               .HasForeignKey(x => x.ResourceId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}