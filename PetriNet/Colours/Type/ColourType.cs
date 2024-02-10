using TACPN.Colours.Expression;
using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

public record ColourType
{
    public static readonly SingletonColourType DefaultColorType = new(Colour.DefaultTokenColour, new ("dot"));
    
    public string Name { get; init; }
    public IReadOnlyCollection<Colour> Colours { get; init; }
    public IEnumerable<string> ColourNames => Colours.Select(e => e.Value);
    
    public ColourType(string name, IEnumerable<string> colours)
    {
        this.Name = name;
        this.Colours = colours.Select(colour=>new Colour(colour)).ToList();
    }
    public ColourType(string name, params string[] colours)
    {
        this.Name = name;
        this.Colours = colours.Select(colour=>new Colour(colour)).ToList();
    }
    
    public ColourType(string name, IEnumerable<Colour> colours)
    {
        this.Name = name;
        this.Colours = colours.Select(colour=>colour).ToList();
    }
    
    public override string ToString()
    {
        return this.Name;
    }

    public bool Contains(string colour)
    {
        return this.Colours.Select(e => e.Value).Contains(colour);
    }

    public bool IsCompatibleWith(IColourValue value)
    {
        return value switch
        {
            IColourTypedValue variable => variable.ColourType == this,
            Colour c => this.Colours.Contains(c),
            _ => false
        };
    }

    public static ColourType FromValues(IEnumerable<string> values)
    {
        return new ColourType("Values", values);
    }
}
