using Common;

namespace Tmpms.Common.Json.Models;

public record LocationDefinition(string Name, int Capacity, List<InvariantDefinition> Invariants, bool IsProcessing)
{
    public static LocationDefinition EndLocation(IEnumerable<string> parts)
    {
        var invariantsForAllParts = parts.Select(e => new InvariantDefinition(e, 0, InfinityInteger.Positive));
        return new LocationDefinition("END", InfinityInteger.Positive, invariantsForAllParts.ToList(), true);
    } 
}