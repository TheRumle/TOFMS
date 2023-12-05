using TACPN.Net.Transitions;
using Tofms.Common.Move;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;


public class MoveActionToTransitionFactory : ITransitionAttachableFactory
{
    public MoveActionToTransitionFactory(JourneyCollection journeyCollection)
    {
        this._journeyCollection = journeyCollection;
    }

    public JourneyCollection _journeyCollection { get; set; }

    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction)
    {
        return new EmptyBeforeCapacitorInhibitorAdaption(moveAction.EmptyBefore, _journeyCollection);
    }

    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction)
    {
        if (moveAction.EmptyAfter.Contains(moveAction.From))
            return new FromLocationIsInEmptyAfterAdapter(moveAction.EmptyAfter, moveAction.From, moveAction.PartsToMove, _journeyCollection);

        return new InhibitorFromEmptyAfterAdapter(moveAction.EmptyAfter, _journeyCollection);
    }

    public ITransitionAttachable AdaptFrom(MoveAction moveAction)
    {
        return new FromLocationAdaption(moveAction.From, moveAction.PartsToMove, _journeyCollection);
    }

    public ITransitionAttachable AdaptTo(MoveAction moveAction)
    {
        return new ToTransitionAttacher(moveAction.To, moveAction.PartsToMove, _journeyCollection);
    }
}