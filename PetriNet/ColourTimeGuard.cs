using TACPN.Colours.Expression;
using TACPN.Colours.Type;

namespace TACPN;

public class ColourTimeGuard
{
    private ColourTimeGuard(ColourType colourType, Interval interval)
    {
        ColourType = colourType;
        Interval = interval;
    }
    
    public IColourValue ColourValue { get; private set; }
    public ColourType ColourType { get; private set; }

    public Interval Interval { get; internal set; }
    
    public static ColourTimeGuard CapacityGuard()
    {
        return new ColourTimeGuard(ColourType.DefaultColorType, Interval.ZeroToInfinity);
    }

    public static ColourTimeGuard TokensGuard(int from, int to, ColourType partsColourType)
    {
        return new ColourTimeGuard(partsColourType, new Interval(from,to));
    }

    public override string ToString()
    {
        return $"T:{ColourType.Name}: {Interval}, {ColourValue.Value}";
    }
}