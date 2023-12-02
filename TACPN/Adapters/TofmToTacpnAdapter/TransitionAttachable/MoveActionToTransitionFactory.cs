using TACPN.Net.Transitions;
using Tofms.Common.Move;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;


public class MoveActionToTransitionFactory : ITransitionAttachableFactory
{
    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction)
    {
        return new EmptyBeforeCapacitorInhibitorAdaption(moveAction.EmptyBefore);
    }

    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction)
    {
        if (moveAction.EmptyAfter.Contains(moveAction.From))
            return new FromLocationIsInEmptyAfterAdapter(moveAction.EmptyAfter, moveAction.From, moveAction.PartsToMove);

        return new InhibitorFromEmptyAfterAdapter(moveAction.EmptyAfter);
    }

    public ITransitionAttachable AdaptFrom(MoveAction moveAction)
    {
        return new FromLocationAdaption(moveAction.From, moveAction.PartsToMove);
    }

    public ITransitionAttachable AdaptTo(MoveAction moveAction)
    {
        return new ToTransitionAttacher(moveAction.To, moveAction.PartsToMove);
    }
}