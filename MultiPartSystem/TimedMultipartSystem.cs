using Tmpms.Journey;
using Tmpms.Move;

namespace Tmpms;

public class TimedMultipartSystem
{
    public required HashSet<Location> Locations { get; init; }
    public required JourneyCollection Journeys { get; init; }
    public required IEnumerable<MoveAction> MoveActions { get; init; }
    public required IEnumerable<string> Parts { get; init; }
    public const string PRODUCTNAME = "Part";
}