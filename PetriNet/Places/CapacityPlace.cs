using TACPN.Colours;
using TACPN.Colours.Type;

namespace TACPN.Places;

public class CapacityPlace : Place
{
    public bool Equals(CapacityPlace other)
    {
        return Name == other.Name && IsCapacityLocation == other.IsCapacityLocation && ColourType.Equals(other.ColourType) && IsProcessingPlace == other.IsProcessingPlace;
    }


    public TokenCollection Tokens { get; }

    public CapacityPlace(string name, int capacity) : base(name, [ColourInvariant.DotDefault], ColourType.DefaultColorType )
    {
        IsCapacityLocation = true;
        IsProcessingPlace = false;
        Name = name;
        Tokens = TokenCollection.DotColorTokenCollection(capacity);
    }

    public override string ToString()
    {
        return this.Name;
    }
}