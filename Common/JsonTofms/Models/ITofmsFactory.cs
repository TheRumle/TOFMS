using Common.Move;

namespace Common.JsonTofms.Models;

public interface ITofmsFactory
{
    public IEnumerable<MoveAction> CreateMoveActions(TofmSystem tofmSystem);
}