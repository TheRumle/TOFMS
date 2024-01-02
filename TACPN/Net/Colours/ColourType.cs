namespace TACPN.Net.Colours;

public enum SingletonColourNamingConvention
{
    ExactSameAsValue,
    CapitalizeFirstLetter
}

public class ColourType
{
    public string Name { get; init; }
    private IReadOnlyCollection<Colour> Colours { get; init; }
    
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

    public static ColourType SingletonColour(string colour, SingletonColourNamingConvention strategy)
    {
        return strategy switch
        {
            SingletonColourNamingConvention.ExactSameAsValue => new ColourType(colour, new List<string> { colour }),
            SingletonColourNamingConvention.CapitalizeFirstLetter => new ColourType(colour,
                new List<string> { char.ToUpper(colour[0]) + colour[1..] }),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, "No such strategy")
        };
    }

    public static readonly ColourType DefaultColorType = ColourType.SingletonColour("dot", SingletonColourNamingConvention.CapitalizeFirstLetter);
    public static readonly ColourType TokensColourType = ColourType.SingletonColour("Tokens", SingletonColourNamingConvention.ExactSameAsValue);

}