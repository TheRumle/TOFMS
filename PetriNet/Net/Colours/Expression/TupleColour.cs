using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class TupleColour : IColourTypedColourValue
{
    public string Value { get; }
    
    public TupleColour(IEnumerable<IColourValue> colours, ColourType colourType)
    {
        IColourValue[] colourValues = colours as IColourValue[] ?? colours.ToArray();
        
        GuardFrom.InvalidColourTypeAssignment(colourValues, colourType);
        this.ColourComponents = colourValues;
        this.ColourType = colourType;
        var values = colours.Select(e => e.Value);
        Value = $"({string.Join(", ", values)})";
    }


    public ColourType ColourType { get; set; }
    public IColourValue[] ColourComponents { get; set; }
}