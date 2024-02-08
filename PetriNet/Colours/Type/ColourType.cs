using TACPN.Colours.Expression;
using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

public record ColourType
{
    public static readonly SingletonColourType DefaultColorType = SingletonColour(Colour.DefaultTokenColour, "dot");

    public static ProductColourType TokensColourType(IEnumerable<string> parts)
    {
        return new ProductColourType("Tokens", DefaultColorType, PartsColourType(parts));
    }
    
    public static ProductColourType TokensColourType(ColourType partsColourType)
    {
        return new ProductColourType("Tokens", DefaultColorType, partsColourType);
    }

    public static ColourType PartsColourType(IEnumerable<string> parts)
    {
        return new ColourType("Parts", parts);
    }
    
    public static ColourType PartsColourType(params string[] parts)
    {
        return new ColourType("Parts", parts);
    }
    public static readonly string JourneyColourName = "Journey";
    
    
    public static string CreateJourneyNameFor(string partType)
    {
        return partType + JourneyColourName;
    }
    
    
    
    
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

    public static IntegerRangedColour JourneyColourFor(string part, int journeyLength)
    {
        return new IntegerRangedColour(CreateJourneyNameFor(part), journeyLength);
    }
}

public record ProductColourType : ColourType
{
    private ProductColourType(string name, IEnumerable<Colour> colours) : base(name, colours)
    {
    }

    public ProductColourType(ColourType first, ColourType second):this(first.Name+second.Name, [..first.Colours, ..second.Colours])
    {
        this.First = first;
        this.Second = second;
    }

    
    public ProductColourType(string name, ColourType first, ColourType second):this(first.Name+second.Name, [..first.Colours, ..second.Colours])
    {
        this.First = first;
        this.Second = second;
    }

    public ColourType Second { get; set; }

    public ColourType First { get; set; }
}