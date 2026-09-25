namespace ResourceAllocation.Domain.Models;

public class ResourceType
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Resource> Resources { get; set; } = [];
}