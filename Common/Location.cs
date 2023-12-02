namespace Tofms.Common;

public class Location
{
    public Location(string name, int capacity, IEnumerable<Invariant> invariants)
    {
        Invariants = new HashSet<Invariant>(invariants);
        Capacity = capacity;
        Name = name;
    }

    public string Name { get; init; }
    public int Capacity { get; init; }
    public IReadOnlySet<Invariant> Invariants { get; init; }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Capacity)}: {Capacity}, {nameof(Invariants)}: {Invariants}";
    }
}