
using TACPN.Net.Colours.Evaluatable;

namespace TACPN.Net.Colours.Type;

public class ColourType
{
    public string Name { get; init; }
    public IReadOnlyCollection<Colour> Colours { get; init; }
    
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

    public static SingletonColourType SingletonColour(string colour, SingletonColourType.NamingConvention strategy)
    {
        return strategy switch
        {
            SingletonColourType.NamingConvention.ExactSameAsValue => new SingletonColourType(colour, new Colour(colour)),
            SingletonColourType.NamingConvention.CapitalizeFirstLetter => new SingletonColourType(colour, new Colour(char.ToUpper(colour[0]) + colour[1..] )),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "No such strategy")
        };
    }

    public static readonly SingletonColourType DefaultColorType = SingletonColour(Colour.DefaultTokenColour, SingletonColourType.NamingConvention.CapitalizeFirstLetter);
    public static readonly ColourType TokensColourType = new("Tokens", new []{Colour.PartsColour, Colour.JourneyColour});
    public static readonly SingletonColourType PartsColourType = SingletonColour(Colour.TokensColour, SingletonColourType.NamingConvention.ExactSameAsValue);
    public static readonly ColourType TokenAndDefaultColourType = UnionColour("TokensDot", new []{DefaultColorType, TokensColourType});

    public static ColourType UnionColour(string name, IEnumerable<ColourType> over)
    {
        var colours = over.SelectMany(e => e.Colours).DistinctBy(e => e.Value);
        return new ColourType(name,colours);
    }

    public override string ToString()
    {
        return this.Name;
    }
    
    
}