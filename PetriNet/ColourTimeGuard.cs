using TACPN.Colours.Expression;
using TACPN.Colours.Type;

namespace TACPN;

public class ColourTimeGuard
{
    public ColourTimeGuard(ColourType colourType, Interval interval)
    {
        ColourType = colourType;
        Interval = interval;
    }
    
    public IColourValue ColourValue { get; private set; }
    public ColourType ColourType { get; private set; }

    public Interval Interval { get; internal set; }
    
        
    public static ColourTimeGuard Default()
    {
        return new ColourTimeGuard(ColourType.DefaultColorType, Interval.ZeroToInfinity);
    }
    
  

    public override string ToString()
    {
        return $"T:{ColourType.Name}: {Interval}, {ColourValue.Value}";
    }
}