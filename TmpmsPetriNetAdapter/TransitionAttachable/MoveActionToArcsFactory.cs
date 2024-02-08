using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;

namespace TmpmsPetriNetAdapter.TransitionAttachable;


public class MoveActionToArcsFactory : ITransitionAttachableFactory
{
    private readonly ColourType _partsColour;

    public MoveActionToArcsFactory(IndexedJourneyCollection indexedJourneyCollection, ColourType partsColourType)
    {
        this.IndexedJourneyCollection = indexedJourneyCollection;
        this._partsColour = partsColourType;
    }

    public IndexedJourneyCollection IndexedJourneyCollection { get; set; }

    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction)
    {
        return new EmptyBeforeCapacitorInhibitorAdaption(moveAction.EmptyBefore, IndexedJourneyCollection,_partsColour);
    }

    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction)
    {
        return new EmptyAfterAdapter(moveAction.EmptyAfter, moveAction.From, moveAction.PartsToMove, IndexedJourneyCollection,_partsColour);
    }

    public ITransitionAttachable AdaptFrom(MoveAction moveAction)
    {
        return new FromLocationAdaption(moveAction.From, moveAction.PartsToMove, IndexedJourneyCollection,_partsColour);
    }

    public ITransitionAttachable AdaptTo(MoveAction moveAction)
    {
        return new ToTransitionAttacher(moveAction.To, moveAction.PartsToMove, IndexedJourneyCollection,_partsColour);
    }
}