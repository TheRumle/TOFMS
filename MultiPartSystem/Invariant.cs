using Common;

namespace Tmpms;

public record Invariant(string PartType, int Min, int Max)
{

    public static Invariant InfinityInvariantFor(string partType) =>
        new Invariant(partType, 0, InfinityInteger.Positive);
    
    public static IEnumerable<Invariant> InfinityInvariantsFor(params string[] partTypes) => partTypes.Select(part => new Invariant(part, 0, InfinityInteger.Positive));
    public static IEnumerable<Invariant> InfinityInvariantsFor(IEnumerable<string> partTypes) => partTypes.Select(part => new Invariant(part, 0, InfinityInteger.Positive));

}