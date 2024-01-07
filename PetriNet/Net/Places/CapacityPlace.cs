
using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Places;

public class CapacityPlace : IPlace<string>
{
    public bool Equals(CapacityPlace other)
    {
        return Name == other.Name && IsCapacityLocation == other.IsCapacityLocation && ColourType.Equals(other.ColourType) && IsProcessingPlace == other.IsProcessingPlace;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CapacityPlace)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, IsCapacityLocation, ColourType, IsProcessingPlace);
    }

    public TokenCollection Tokens { get; }
    public string Name { get; init; }
    public bool IsCapacityLocation { get; init; }
    public ColourType ColourType { get; }
    public bool IsProcessingPlace { get; init; }
    public IEnumerable<ColourInvariant<string>> ColourInvariants { get; init; }

    public CapacityPlace(string name, int capacity)
    {
        IsCapacityLocation = true;
        IsProcessingPlace = false;
        ColourInvariants = new List<ColourInvariant<string>>(){ColourInvariant.DotDefault};
        ColourType = ColourType.DefaultColorType;
        Name = name;
        Tokens = TokenCollection.DotColorTokenCollection(capacity);
    }

    public override string ToString()
    {
        return this.Name;
    }
}