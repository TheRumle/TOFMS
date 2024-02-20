using Tmpms;

namespace TmpmsChecker.Algorithm;

public class Configuration
{
    private readonly Dictionary<Location, LocationConfiguration> _locationConfigurations = new();

    internal Configuration(IEnumerable<KeyValuePair<Location, LocationConfiguration>> locationConfigurations)
    {
        _locationConfigurations = locationConfigurations.ToDictionary();
    }

    public IReadOnlyDictionary<Location, LocationConfiguration> LocationConfigurations =>
        _locationConfigurations.AsReadOnly();

    public virtual bool Equals(Configuration? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _locationConfigurations.SequenceEqual(other._locationConfigurations);
    }

    public override int GetHashCode()
    {
        return _locationConfigurations.GetHashCode();
    }

    public Configuration Copy()
    {
        return new Configuration(_locationConfigurations.Select(kvp =>
            KeyValuePair.Create(kvp.Key, kvp.Value.Copy())));
    }

    public void Add(Location location, LocationConfiguration configuration)
    {
        if (_locationConfigurations.TryGetValue(location, out _))
            throw new ArgumentException(
                $"{location.Name} already have a registered configuration. Use Replace if you want to replace it");
        _locationConfigurations[location] = configuration;
    }
    
    //Replaces the existing value
    public void Replace(Location location, LocationConfiguration configuration)
    {
        _locationConfigurations[location] = configuration;
    }

    public int RemainingCapacityFor(Location location)
    {
        return location.Capacity - _locationConfigurations[location].Size;
    }
}