using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Colours.Evaluatable;

public record struct Colour (string Value) : IColourExpressionEvaluatable 
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