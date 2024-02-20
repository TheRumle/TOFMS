using Common;

namespace Tmpms;

public record Invariant(string PartType, int Min, int Max)
{
    public static Invariant InfinityInvariantFor(string partType) => new(partType, 0, InfinityInteger.Positive);
}