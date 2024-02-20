using System.ComponentModel.DataAnnotations.Schema;

namespace Tmpms;

public interface ILocation
{
    bool IsProcessing { get; set; }
    string Name { get; init; }
    int Capacity { get; init; }
}

public record Location : ILocation
{
    public LocationConfiguration Configuration;
    
    public static string EndLocationName = "End";
    public bool IsProcessing { get; set; }
    
    public Location(string name, int capacity, IEnumerable<Invariant> invariants, bool isProc, IEnumerable<string> parts)
    {
        Invariants = new HashSet<Invariant>(invariants);
        Capacity = capacity;
        Name = name;
        this.IsProcessing = isProc;
        this.Configuration = new LocationConfiguration(parts);
    }

    public string Name { get; init; }
    public int Capacity { get; init; }
    public IReadOnlySet<Invariant> Invariants { get; init; }

    public override string ToString()
    {
        return $"{Name}, {nameof(Capacity)}: {Capacity}, {nameof(Invariants)}: {Invariants}";
    }

    public IReadOnlyDictionary<string, Invariant> InvariantsByType => Invariants
        .Select(invariant => KeyValuePair.Create(invariant.PartType, invariant)).ToDictionary();

}