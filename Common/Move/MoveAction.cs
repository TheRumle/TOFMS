namespace Tofms.Common.Move;

public record MoveAction
{
    public ISet<Location> EmptyAfter { get; init; }

    public ISet<Location> EmptyBefore { get; init; }

    public string Name { get; init; }

    public Location To { get; init; }
    public Location From { get; init; }
    public HashSet<KeyValuePair<string, int>> PartsToMove { get; init; }

    public IEnumerable<Location> InvolvedLocations => GetAllLocations();

    private IEnumerable<Location> GetAllLocations()
    {
        var locations = new HashSet<Location>();
        locations.UnionWith(EmptyAfter);
        locations.UnionWith(EmptyBefore);
        locations.Add(To);
        locations.Add(From);
        return locations;
    }
}