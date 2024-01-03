using TACPN.Net.Transitions;
using Tmpms.Common.Move;

namespace TACPN.Adapters.TmpmsToTacpnAdapter;

public interface ITransitionAttachableFactory
{
    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction);
    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction);
    public ITransitionAttachable AdaptFrom(MoveAction moveAction);
    public ITransitionAttachable AdaptTo(MoveAction moveAction);
}