using Tmpms.Common.Move;

namespace Tmpms.Common.Json.Models;

public interface ITofmsFactory
{
    public IReadOnlyCollection<MoveAction> CreateMoveActions(TofmJsonSystem tofmJsonSystem);
}