using Tmpms.Common;

namespace Xml;

public record CapacityLocation : ILocation
{
    public CapacityLocation(string name, int capacity)
    {
        Name = name + "_buffer";
        Capacity = capacity;
        IsProcessing = false;
        this.Invariant = LocationExtensions.Infinity;
    }

    public Invariant Invariant { get; set; }


    public bool IsProcessing { get; set; }
    public string Name { get; init; }
    public int Capacity { get; init; }
}