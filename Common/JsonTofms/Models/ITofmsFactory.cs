using Tofms.Common.Move;

namespace Tofms.Common.JsonTofms.Models;

public interface ITofmsFactory
{
    public IReadOnlyCollection<MoveAction> CreateMoveActions(TofmJsonSystem tofmJsonSystem);
}