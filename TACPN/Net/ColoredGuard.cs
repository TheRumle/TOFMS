using TACPN.Net.Colours;

namespace TACPN.Net;

public class ColoredGuard
{
    private ColoredGuard(int amount, ColourType color, Interval interval)
    {
        Amount = amount;
        Color = color;
        Interval = interval;
    }

    public ColourType Color { get; set; }

    public Interval Interval { get; }
    public int Amount { get; }
    
    public static ColoredGuard CapacityGuard(int amount)
    {
        return new ColoredGuard(amount, ColourType.DefaultColorType, Interval.ZeroToInfinity);
    }

    public static ColoredGuard TokensGuard(int amount, int from, int to)
    {
        return new ColoredGuard(amount, ColourType.DefaultColorType, new Interval(from, to));
    } 

}