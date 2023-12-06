namespace TACPN.Net;

public class ColourType
{
    public static readonly ColourType DefaultColorType = new ColourType("Dot", new[] { "dot" });
    public static readonly ColourType TokensColourType = new ColourType("Tokens",new []{"Tokens"});



    public static ColourType CreatePartColourType(IEnumerable<string> partNames)
    {
        return new ColourType("Parts", partNames);
    }

    private ColourType(string Name, IEnumerable<string> Colours)
    {
        this.Name = Name;
        this.Colours = Colours;
    }

    public string Name { get; init; }
    public IEnumerable<string> Colours { get; init; }

    public void Deconstruct(out string Name, out IEnumerable<string> Colours)
    {
        Name = this.Name;
        Colours = this.Colours;
    }
}