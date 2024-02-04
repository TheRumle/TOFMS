using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter.TransitionAttachable;


public class MoveActionToArcsFactory : ITransitionAttachableFactory
{
    public MoveActionToArcsFactory(IndexedJourneyCollection indexedJourneyCollection)
    {
        this.IndexedJourneyCollection = indexedJourneyCollection;
    }

    public IndexedJourneyCollection IndexedJourneyCollection { get; set; }

    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction)
    {
        return new EmptyBeforeCapacitorInhibitorAdaption(moveAction.EmptyBefore, IndexedJourneyCollection);
    }

    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction)
    {
        return new EmptyAfterAdapter(moveAction.EmptyAfter, moveAction.From, moveAction.PartsToMove, IndexedJourneyCollection);
    }

    public ITransitionAttachable AdaptFrom(MoveAction moveAction)
    {
        return new FromLocationAdaption(moveAction.From, moveAction.PartsToMove, IndexedJourneyCollection);
    }

    public ITransitionAttachable AdaptTo(MoveAction moveAction)
    {
        return new ToTransitionAttacher(moveAction.To, moveAction.PartsToMove, IndexedJourneyCollection);
    }
}