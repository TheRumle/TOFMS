namespace Tmpms;

public record Configuration
{
    public virtual bool Equals(Configuration? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _locationConfigurations.SequenceEqual(other._locationConfigurations) && PartTypes.SequenceEqual(other.PartTypes);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_locationConfigurations, PartTypes);
    }

    private readonly Dictionary<Location, LocationConfiguration> _locationConfigurations = new();
    public readonly IEnumerable<string> PartTypes;
    public IReadOnlyDictionary<Location, LocationConfiguration> LocationConfigurations => _locationConfigurations.AsReadOnly();

    private Configuration(IEnumerable<KeyValuePair<Location, LocationConfiguration>> copies)
    {
        _locationConfigurations = copies.ToDictionary();
    }
    
    public Configuration Copy()
    {
        return new Configuration(this._locationConfigurations.Select(kvp =>
            KeyValuePair.Create(kvp.Key, kvp.Value.Copy())));
    }
}