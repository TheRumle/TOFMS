using TACPN.Colours;
using TACPN.Colours.Type;

namespace TACPN.Places;


public class Place : IPlace
{
    public string Name { get; set; }
    public bool IsCapacityLocation { get; set; }
    public ColourType ColourType { get; init; }
    public bool IsProcessingPlace { get; set;  }
    public IEnumerable<ColourInvariant> ColourInvariants { get; set; }

    public Place(string name, IEnumerable<ColourInvariant> colourInvariants, ColourType colourType)
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
