using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Values;

public record VariableIncrement : IColourVariableExpression
{
    public VariableIncrement(ColourVariable colourVariable)
    {

        this.ColourVariable = colourVariable;
        this.ColourType = colourVariable.ColourType;
        Value = $"{ColourVariable.Value}--";
    }

    public string Value { get; set; }

    public ColourVariable ColourVariable { get; set; }


    public static implicit operator string(VariableIncrement color)
    {
        return color.Value;
    }

    public ColourType ColourType { get; }
}