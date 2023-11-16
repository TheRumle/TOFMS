namespace Common;

public class Infteger
{
    public string Name { get; }
    public static readonly Infteger PositiveInfinity = new Infteger("inf");
    public static readonly Infteger NegativeInfinity = new Infteger("inf");

    private Infteger(string name)
    {
        this.Name = name;
    }

    public static implicit operator int(Infteger value)
    {
        return value == PositiveInfinity ? Int32.MaxValue : int.MinValue;
    }
}

public static class InfinityInteger
{
    public static readonly Infteger Positive = Infteger.PositiveInfinity;
    public static readonly Infteger Negative = Infteger.PositiveInfinity;

    public static bool IsPositiveInfinity(this int x)
    {
        return x == Positive;
    }

    public static bool IsNegativeInfinity(this int x)
    {
        return x == Negative;
    }  
}