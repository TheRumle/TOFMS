using System.Runtime.CompilerServices;
using TACPN.Colours.Type;
using TACPN.Places;

namespace TACPN.Colours.Values;

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

    public bool IsVariableFor(string partType)
    {
        return ColourVariable.VariableNameFor(partType) == Name;
    }
    
    public static IEnumerable<VariableDecrement> DecrementsFor(IEnumerable<string> parts)
    {
        IEnumerable<VariableDecrement> a = parts.Select(e => new VariableDecrement(new ColourVariable(VariableNameFor(e),
            ColourType.PartsColourType(parts))));
        return a;
    }
    public static VariableDecrement DecrementFor(string part, ColourType partColourType)
    {
        return new VariableDecrement(new ColourVariable(VariableNameFor(part), partColourType));
    }
    
    public static VariableDecrement DecrementForPartType(string part, int journeyLenght)
    {
        return new VariableDecrement(new ColourVariable(VariableNameFor(part), Type.ColourType.JourneyColourFor(part, journeyLenght)));
    }

    public string Name { get; }
    public string Value { get; }
    public ColourType ColourType { get; }

    public static ColourVariable Create(string name, ColourType colourType)
    {
        return CreateIfValid(name, colourType);
    }

    private static ColourVariable CreateIfValid(string name, ColourType colourType)
    {
        if (colourType.Colours.Any(e=>e.Value == name)) 
            throw new ArgumentException($"Cannot create variable with same name as assigned colour type: {string.Join(" ", colourType.Colours.Select(e=>e.Value))}");

        return new ColourVariable(name, colourType);
    }

    public static ColourVariable CreateFromPartName(string part, int journeyLength)
    {
        var ct = Type.ColourType.JourneyColourFor(part, journeyLength+1);
        return new ColourVariable(VariableNameFor(part), ct);
    }

    ColourVariable IColourVariableExpression.ColourVariable => this;
}