using Common.Move;
using TACPN.Net.Transitions;

namespace TACPN.Adapters.Location;

public interface ITransitionAttachableFactory
{
    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction);
    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction);
    public ITransitionAttachable AdaptFrom(MoveAction moveAction);
    public ITransitionAttachable AdaptTo(MoveAction moveAction);
}