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
}