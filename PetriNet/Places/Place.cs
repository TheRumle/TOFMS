using TACPN.Colours;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;

namespace TACPN.Places;

public class Place : IPlace
{
    public readonly Marking Marking = new();
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
        return Name;
    }

    public void AddTokenOfColour(IColourValue colorValue)
    {
        AssertOkayToAdd(colorValue);
        AddToken((colorValue,0));
    }
    public void AddToken((IColourValue colorValue, int age) token)
    {
        AssertOkayToAdd(token.colorValue); 
        Marking.AddToken(token.colorValue,token.age);
    }
    
    public void AddTokenOfColour(IEnumerable<IColourValue> values)
    {
        var vals = values.ToArray();
        vals.All(AssertOkayToAdd);
        foreach (var colorValue in vals)
           AddToken((colorValue,0));
    }

    private bool AssertOkayToAdd(IColourValue colorValue)
    {
        if (!ColourType.IsCompatibleWith(colorValue))
            throw new ArgumentException(
                $"{colorValue.Value} is not compatible with {this.Name}'s colour type {ColourType.Name}");
        return true;
    }
}
