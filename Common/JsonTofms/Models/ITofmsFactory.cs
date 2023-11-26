using Tofms.Common.Move;

namespace Tofms.Common.JsonTofms.Models;

public interface ITofmsFactory
{
    public IReadOnlyList<MoveAction> CreateMoveActions(TofmSystem tofmSystem);
}