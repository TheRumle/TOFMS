namespace Common;

public class Location
{
    public string Name { get; init; }
    public int Capacity { get; init; }
    public IReadOnlySet<Invariant> Invariants { get; init; }

    public Location(string name, int capacity, IEnumerable<Invariant> invariants)
    {
        this.Invariants = new HashSet<Invariant>(invariants);
        this.Capacity = capacity;
        this.Name = name;
    }
    
    public Location(string name, int capacity, Invariant invariant)
    {
        this.Invariants = new HashSet<Invariant>()
        {
            invariant
        };
        this.Capacity = capacity;
        this.Name = name;
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Capacity)}: {Capacity}, {nameof(Invariants)}: {Invariants}";
    }
}