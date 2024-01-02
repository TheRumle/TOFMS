using TACPN.Net.Colours;
using TACPN.Net.Colours.Evaluatable;
using TACPN.Net.Colours.Type;

namespace TACPN.Net;

public class ColoredGuard
{
    private ColoredGuard(ColourType colourType, Colour colour, Interval interval)
    {
        ColourType = colourType;
        Interval = interval;
        this.Colour = colour;
    }
    
    public Colour Colour { get; private set; }
    public ColourType ColourType { get; private set; }

    public Interval Interval { get; }
    
    public static ColoredGuard CapacityGuard()
    {
        return new ColoredGuard(ColourType.DefaultColorType, ColourType.DefaultColorType.Colours.First(), Interval.ZeroToInfinity);
    }

    public static ColoredGuard TokensGuard(int from, int to)
    {
        return new ColoredGuard(ColourType.TokensColourType, ColourType.TokensColourType.Colours.First(), new Interval(from, to));
    } 

}