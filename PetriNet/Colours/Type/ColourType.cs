using TACPN.Colours.Expression;
using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

public class ColourType
{
    public static readonly SingletonColourType DefaultColorType = SingletonColour(Colour.DefaultTokenColour, "Dot");
    public static readonly ColourType TokensColourType = new("Tokens", new []{Colour.PartsColour, Colour.JourneyColour});
    public static readonly ColourType PartsColourType = TokensColourType;
    public static readonly ColourType TokenAndDefaultColourType = ColourProduct("TokensDot", new []{DefaultColorType, TokensColourType});
    public static readonly string JourneyColourName = "Journey";
    public string Name { get; init; }
    public IReadOnlyCollection<Colour> Colours { get; init; }
    public IEnumerable<string> ColourNames => Colours.Select(e => e.Value);
    
    public ColourType(string name, IEnumerable<string> colours)
    {
        this.Name = name;
        this.Colours = colours.Select(colour=>new Colour(colour)).ToList();
    }
    public ColourType(string name, IEnumerable<Colour> colours)
    {
        this.Name = name;
        this.Colours = colours.Select(colour=>colour).ToList();
    }

    public static SingletonColourType SingletonColour(Colour colour, string name)
    {
        return new SingletonColourType(name, colour);
    }

   

    private static ColourType ColourProduct(string name, IEnumerable<ColourType> over)
    {
        var colours = over.SelectMany(e => e.Colours).DistinctBy(e => e.Value);
        return new ColourType(name,colours);
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

    public static ColourExpression TokenColourExpression(int amount)
    {
        return new ColourExpression(TokensColourType.Colours.First(), TokensColourType, amount);

    }
    
}