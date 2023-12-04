namespace TACPN.Net;

public class ColoredGuard
{
    public ColoredGuard(int amount, string color, Interval interval)
    {
        Amount = amount;
        Color = color;
        Interval = interval;
    }

    public string Color { get; set; }

    public Interval Interval { get; }
    public int Amount { get; }
    
    public static ColoredGuard CapacityGuard(int amount)
    {
        return new ColoredGuard(amount, CapacityPlaceExtensions.DefaultCapacityColor, Interval.ZeroToInfinity);
    }


}