using Common;

namespace TACPN.IntegrationTests;

public static class ExampleLocations
{
    public static readonly Location CBuffer = new("CBuffer", 4, new[]
    {
        new Invariant("P1", 0, InfinityInteger.Positive),
        new Invariant("P2", 0, InfinityInteger.Positive)
    });

    public static readonly Location CProc = new("CProc", 4, new[]
    {
        new Invariant("P1", 7, InfinityInteger.Positive),
        new Invariant("P2", 12, InfinityInteger.Positive)   
    });
}