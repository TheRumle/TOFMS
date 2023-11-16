using Common.Move;

namespace Common.JsonTofms.Models;

public interface ITofmsFactory
{
    public IReadOnlyList<MoveAction> CreateMoveActions(TofmSystem tofmSystem);
}