using Tmpms;
using Tmpms.Move;

namespace JsonFixtures;

public class MoveActionFactoryFixture
{
    public MoveAction CreateMoveAction(Location from, Location to, HashSet<Location> emptyBefore, HashSet<Location> emptyAfter, IEnumerable<(string part, int amount)> partToMove)
    {
        return new MoveAction()
        {
            Name = "Test",
            EmptyAfter = emptyAfter,
            EmptyBefore = emptyBefore,
            From = from,
            To = to,
            PartsToMove = partToMove.Select(n=>KeyValuePair.Create(n.part, n.amount)).ToDictionary()
        };
    }
}