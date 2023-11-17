namespace Common;

public class Infteger
{
    public static readonly Infteger PositiveInfinity = new("inf");
    public static readonly Infteger NegativeInfinity = new("inf");

    private Infteger(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public static implicit operator int(Infteger value)
    {
        return value == PositiveInfinity ? int.MaxValue : int.MinValue;
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