using TACPN.Net.Transitions;
using Tofms.Common.Move;

namespace TACPN.Adapters.TofmToTacpnAdapter;

public interface ITransitionAttachableFactory
{
    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction);
    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction);
    public ITransitionAttachable AdaptFrom(MoveAction moveAction);
    public ITransitionAttachable AdaptTo(MoveAction moveAction);
}