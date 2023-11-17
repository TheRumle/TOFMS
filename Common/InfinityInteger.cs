namespace Common;

public class Infteger
{
    public static readonly Infteger PositiveInfinity = new();
    public static readonly Infteger NegativeInfinity = new();
    
    public static implicit operator int(Infteger value)
    {
        return value == PositiveInfinity ? int.MaxValue : int.MinValue;
    }
}

public static class InfinityInteger
{
    public static readonly Infteger Positive = Infteger.PositiveInfinity;
    public static readonly Infteger Negative = Infteger.PositiveInfinity;
}