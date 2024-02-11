using System.ComponentModel.DataAnnotations.Schema;

namespace Tmpms.Common;

public interface ILocation
{
    bool IsProcessing { get; set; }
    string Name { get; init; }
    int Capacity { get; init; }
}

public record Location : ILocation
{
    //TODO require explicit mapping at some point
    [NotMapped] public LocationConfiguration Configuration = new LocationConfiguration();
    
    public static string EndLocationName = "End";
    public bool IsProcessing { get; set; }
    
    public Location(string name, int capacity, IEnumerable<Invariant> invariants, bool isProc)
    {
        Invariants = new HashSet<Invariant>(invariants);
        Capacity = capacity;
        Name = name;
        this.IsProcessing = isProc;
    }

    public string Name { get; init; }
    public int Capacity { get; init; }
    public IReadOnlySet<Invariant> Invariants { get; init; }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Capacity)}: {Capacity}, {nameof(Invariants)}: {Invariants}";
    }
}