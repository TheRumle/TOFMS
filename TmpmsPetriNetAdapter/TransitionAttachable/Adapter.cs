using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal abstract class Adapter : ITransitionAttachable
{
    public ColourTimeGuardFactory TimeGuardFactory { get; set; }

    public ColourTypeFactory ColourFactory { get; set; }

    protected readonly PlaceFactory PlaceFactory;
    protected ColourType PartColourType { get; }
    protected readonly IndexedJourneyCollection Collection;
    protected readonly ColourExpressionFactory JourneyColourExpressionFactory;
    public Adapter(ColourTypeFactory ctFactory,  IndexedJourneyCollection collection)
    {
        this.ColourFactory = ctFactory;
        this.PartColourType = ctFactory.Parts;
        PlaceFactory = new PlaceFactory(ctFactory);
        Collection = collection;
        JourneyColourExpressionFactory = new ColourExpressionFactory(ctFactory);
        this.TimeGuardFactory = new ColourTimeGuardFactory(ctFactory);
    }
    public abstract void AttachToTransition(Transition transition);

}