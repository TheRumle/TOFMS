using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal abstract class Adapter : ITransitionAttachable
{
    public abstract void AttachToTransition(Transition transition);

    public Adapter(ColourType partColourType,  IndexedJourneyCollection collection)
    {
        this.PartColourType = partColourType;
        PlaceFactory = new PlaceFactory(partColourType, collection);
        _collection = collection;
        journeyColourFactory = new PartJourneyColourFactory(partColourType);
    }
    protected readonly PlaceFactory PlaceFactory;
    protected ColourType PartColourType { get; }
    protected readonly IndexedJourneyCollection _collection;
    protected readonly PartJourneyColourFactory journeyColourFactory;
}