using Tmpms.Move;

namespace TMPMSChecker.Algorithm;

public class Schedule
{
    private Dictionary<int, List<MoveAction>> MovesAtTimeT { get; } = new();

    public IEnumerable<Event> ActionsAtTime(int n)
    {
        if (MovesAtTimeT.TryGetValue(n, out var values))
        {
            return values.Select(e=>new Event(e.Name));
        }
        return [];
    }
}