using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter.TransitionAttachable;


public class MoveActionToArcsFactory : ITransitionAttachableFactory
{
    public MoveActionToArcsFactory(IndexedJourney indexedJourney)
    {
        this.IndexedJourney = indexedJourney;
    }

    public IndexedJourney IndexedJourney { get; set; }

    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction)
    {
        return new EmptyBeforeCapacitorInhibitorAdaption(moveAction.EmptyBefore, IndexedJourney);
    }

    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction)
    {
        return new EmptyAfterAdapter(moveAction.EmptyAfter, moveAction.From, moveAction.PartsToMove, IndexedJourney);
    }

    public ITransitionAttachable AdaptFrom(MoveAction moveAction)
    {
        return new FromLocationAdaption(moveAction.From, moveAction.PartsToMove, IndexedJourney);
    }

    public ITransitionAttachable AdaptTo(MoveAction moveAction)
    {
        return new ToTransitionAttacher(moveAction.To, moveAction.PartsToMove, IndexedJourney);
    }
}