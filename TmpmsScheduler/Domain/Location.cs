

using Tmpms;

public record Location
{
    public LocationConfiguration Configuration;
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
    public IReadOnlyDictionary<string, Invariant> InvariantsByType => Invariants
        .Select(invariant => KeyValuePair.Create(invariant.PartType, invariant)).ToDictionary();

}