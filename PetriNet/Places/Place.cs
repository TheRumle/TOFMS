using TACPN.Colours;
using TACPN.Colours.Type;

namespace TACPN.Places;


public class Place : IPlace<int, string>
{
    public string Name { get; set; }
    public bool IsCapacityLocation { get; set; }
    public ColourType ColourType { get; init; }
    public bool IsProcessingPlace { get; set;  }
    public IEnumerable<ColourInvariant<int, string>> ColourInvariants { get; set; }

    public Place(bool isProcessingPlace, string name, IEnumerable<ColourInvariant<int, string>> colourInvariants, ColourType colourType)
    {
        IsProcessingPlace = isProcessingPlace;
        Name = name;
        ColourInvariants = colourInvariants; 
        IsCapacityLocation = false;
        ColourType = colourType;

    }

    public override string ToString()
    {
        return this.Name;
    }
}
