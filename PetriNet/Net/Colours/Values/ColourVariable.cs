using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Values;

public record ColourVariable : IColourVariableExpression
{
    private ColourVariable(string name, ColourType colourType)
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
    
    public static IEnumerable<VariableDecrement> DecrementsFor(IEnumerable<string> parts)
    {
        IEnumerable<VariableDecrement> a = parts.Select(e => new VariableDecrement(new ColourVariable(VariableNameFor(e),ColourType.TokensColourType)));
        return a;
    }
    public static VariableDecrement DecrementFor(string part)
    {
        return new VariableDecrement(new ColourVariable(VariableNameFor(part), ColourType.TokensColourType));
    }

    public string Name { get; }
    public string Value { get; }
    public ColourType ColourType { get; }

    public static ColourVariable Create(string name, ColourType colourType)
    {
        if (colourType.Colours.Any(e=>e.Value == name)) 
            throw new ArgumentException($"Cannot create variable with same name as assigned colour type: {string.Join(" ", colourType.Colours.Select(e=>e.Value))}");

        return new ColourVariable(name, colourType);

    }

    ColourVariable IColourVariableExpression.ColourVariable => this;
}