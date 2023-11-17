using Common;

namespace TACPN.Net;

public record Interval(int Min, int Max)
{
    public static readonly Interval ZeroToInfinity = new(0, InfinityInteger.Positive);
}