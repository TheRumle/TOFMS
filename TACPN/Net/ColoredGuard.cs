namespace TACPN.Net;

public class ColoredGuard
{
    public ColoredGuard(int amount, string color, Interval interval)
    {
        this.Amount = amount;
        this.Color = color;
        this.Interval = interval;
    }

    public string Color { get; set; }

    public Interval Interval { get; }
    public int Amount { get; }
}