using Tmpms.Common.Move;

namespace Tmpms.Common.JsonTofms.Models;

public interface ITofmsFactory
{
    public IReadOnlyCollection<MoveAction> CreateMoveActions(TofmJsonSystem tofmJsonSystem);
}