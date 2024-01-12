
using Common;

namespace TACPN;

public record Interval(int Min, int Max)
{
    public static readonly Interval ZeroToInfinity = new(0, InfinityInteger.Positive);
}