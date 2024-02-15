using TACPN.Transitions;
using Tmpms.Move;

namespace TmpmsPetriNetAdapter;

public interface ITransitionAttachableFactory
{
    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction);
    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction);
    public ITransitionAttachable AdaptFrom(MoveAction moveAction);
    public ITransitionAttachable AdaptTo(MoveAction moveAction);
}