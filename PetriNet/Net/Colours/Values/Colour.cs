using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Colours.Values;

public record Colour (string Value) : IColourValue 
{
    public static implicit operator string(Colour color)
    {
        return color.Value;
    }

    public static Colour DefaultTokenColour = new Colour("dot");
    public static Colour TokensColour = new Colour("Tokens");
    public static Colour PartsColour = new Colour("Parts");
    public static Colour JourneyColour = new Colour("Journey");
}