namespace TACPN.Net;

public record ColourType(string Name, IEnumerable<string> Colours)
{
    public static readonly ColourType DefaultColorType = new ColourType("Dot", new[] { "dot" });
    public static readonly ColourType TokensColourType = new ColourType("Tokens",new []{"Tokens"});
}