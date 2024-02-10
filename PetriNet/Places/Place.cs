using TACPN.Colours;
using TACPN.Colours.Type;

namespace TACPN.Places;


public class Place : IPlace<string>
{
    public string Name { get; set; }
    public bool IsCapacityLocation { get; set; }
    public ColourType ColourType { get; init; }
    public bool IsProcessingPlace { get; set;  }
    public IEnumerable<ColourInvariant<string>> ColourInvariants { get; set; }

    public Place(string name, IEnumerable<ColourInvariant<string>> colourInvariants, ColourType colourType)
    {
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
