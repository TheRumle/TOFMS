using Common;

namespace TACPN.Net;

public record Interval(int Min, int Max)
{
    public static readonly Interval ZeroToInfinity = new Interval(0, InfinityInteger.Positive);
    
}
