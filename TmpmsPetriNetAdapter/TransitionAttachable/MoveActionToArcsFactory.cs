using TACPN.Transitions;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;


public class MoveActionToArcsFactory : ITransitionAttachableFactory
{
    private readonly ColourTypeFactory _ctFactory;
    public JourneyCollection IndexedJourneyCollection { get; set; }

    public MoveActionToArcsFactory(JourneyCollection indexedJourneyCollection, ColourTypeFactory colourTypeFactory)
    {
        this.IndexedJourneyCollection = indexedJourneyCollection;
        this._ctFactory = colourTypeFactory;
    }


    public ITransitionAttachable AdaptEmptyBefore(MoveAction moveAction)
    {
        return new EmptyBeforeCapacitorInhibitorAdaption(moveAction, _ctFactory, IndexedJourneyCollection);
    }

    public ITransitionAttachable AdaptEmptyAfter(MoveAction moveAction)
    {
        return new EmptyAfterAdapter(moveAction, _ctFactory, IndexedJourneyCollection);
    }

    public ITransitionAttachable AdaptFrom(MoveAction moveAction)
    {
        return new MovingProductsOutOfLocationAttacherTest(moveAction, _ctFactory, IndexedJourneyCollection);
    }

    public ITransitionAttachable AdaptTo(MoveAction moveAction)
    {
        return new MovingProductsIntoLocationAdapter(moveAction, _ctFactory, IndexedJourneyCollection);;
    }
}