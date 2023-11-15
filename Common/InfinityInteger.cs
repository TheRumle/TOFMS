namespace Common;

public static class InfinityInteger
{

    public const int Positive = int.MaxValue;

    public const int Negative = int.MinValue;

    public static bool IsPositiveInfinity(this int x)
    {
        return x == Positive;
    }

    public static bool IsNegativeInfinity(this int x)
    {
        return x == Negative;
    }  
}