using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Values;

public record struct ColourVariable : IColourTypedColourValue
{
    private ColourVariable(string Name, ColourType ColourType)
    {
        this.Name = Name;
        this.ColourType = ColourType;
        this.Value = Name;
    }

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

    public string Name { get; init; }
    public string Value { get; init; }
    public ColourType ColourType { get; set; }

    public static ColourVariable Create(string name, ColourType colourType)
    {
        if (colourType.Colours.Any(e=>e.Value == name)) 
            throw new ArgumentException($"Cannot create variable with same name as assigned colour type: {string.Join(" ", colourType.Colours.Select(e=>e.Value))}");

        return new ColourVariable(name, colourType);

    }
}