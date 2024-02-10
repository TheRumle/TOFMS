using TACPN.Colours.Type;

namespace TACPN.Colours.Values;

public record ColourVariable : IColourVariableExpression
{
    public ColourVariable(string name, ColourType colourType)
    {
        this.Name = name;
        this.ColourType = colourType;
        this.Value = name;
        this.VariableValues = new VariableValueList(colourType);
    }

    public readonly VariableValueList VariableValues;

    public static string VariableNameFor(string part)
    {
        return $"Var{part}";
    }


    public string Name { get; }
    public string Value { get; }
    public ColourType ColourType { get; }


    ColourVariable IColourVariableExpression.ColourVariable => this;
    
}