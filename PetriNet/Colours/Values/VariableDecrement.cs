using TACPN.Colours.Type;

namespace TACPN.Colours.Values;

public record VariableDecrement : IColourVariableExpression
{
    public VariableDecrement(ColourVariable colourVariable)
    {

        this.ColourVariable = colourVariable;
        this.ColourType = colourVariable.ColourType;
        Value = $"{ColourVariable.Value}--";
    }

    public ColourVariable ColourVariable { get; set; }

    public string Value { get; set; }
    public ColourType ColourType { get; init; }

    public override string ToString()
    {
        return this.Value;
    }
}