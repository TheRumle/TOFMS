using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker;

public static class DomainExtensions
{
    public static int TotalAmountToMove(this MoveAction action)
    {
        return action.PartsToMove.Select(e => e.Value).Sum();
    }
}