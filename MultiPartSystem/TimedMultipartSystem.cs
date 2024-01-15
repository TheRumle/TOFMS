using Tmpms.Common.Move;

namespace Tmpms.Common;

public class TimedMultipartSystem
{
    public required IReadOnlyDictionary<string, IEnumerable<Location>> Journeys { get; init; }
    public required IEnumerable<MoveAction> MoveActions { get; init; }
    public required IEnumerable<string> Parts { get; init; }
    
}