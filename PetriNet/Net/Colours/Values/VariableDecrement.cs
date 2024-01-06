using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Values;

public record VariableDecrement : IColourTypedValue
{
    public VariableDecrement(ColourVariable colourVariable)
    {

        this.ColourVariable = colourVariable;
        this.ColourType = colourVariable.ColourType;
        Value = $"{ColourVariable.Value}--";
        this.VariableName = ColourVariable.Name;
    }

    public ColourVariable ColourVariable { get; set; }

    public string Value { get; set; }
    public string VariableName { get; init; }
    public ColourType ColourType { get; init; }

    public override string ToString()
    {
        return this.Value;
    }
}