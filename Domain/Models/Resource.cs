namespace ResourceAllocation.Domain.Models;

public class Resource
{
    public Guid Id { get; set; }

    public Guid TypeId { get; set; }
    public ResourceType Type { get; set; } = null!;

    public required string Name { get; set; }

    public ICollection<Allocation> Allocations { get; set; } = [];
}