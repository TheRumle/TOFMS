using Tmpms;
using Tmpms.Move;
using MoveAction = TmpmsChecker.Domain.MoveAction;

namespace TmpmsChecker;

public static class DomainExtensions
{
    public static int TotalAmountToMove(this MoveAction action)
    {
        return action.PartsToMove.Select(e => e.Value).Sum();
    }
}