using TACPN.Colours.Expression;
using TACPN.Colours.Type;

namespace TACPN;

public class ColourTimeGuard
{
    private ColourTimeGuard(ColourType colourType, IColourValue colourValue, Interval interval)
    {
        ColourType = colourType;
        Interval = interval;
        this.ColourValue = colourValue;
    }
    
    public IColourValue ColourValue { get; private set; }
    public ColourType ColourType { get; private set; }

    public Interval Interval { get; internal set; }
    
    public static ColourTimeGuard CapacityGuard()
    {
        return new ColourTimeGuard(ColourType.DefaultColorType, ColourType.DefaultColorType.Colours.First(), Interval.ZeroToInfinity);
    }

    public static ColourTimeGuard TokensGuard(int from, int to)
    {
        return new ColourTimeGuard(ColourType.TokensColourType, ColourType.TokensColourType.Colours.First(), new Interval(from, to));
    } 

}