using TACPN.Transitions;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class InhibitorFromEmptyAfterAdapter : ITransitionAttachable
{
    private readonly IEnumerable<Location> _emptyAfterLocations;
    private readonly JourneyCollection _journeyCollection;

    public InhibitorFromEmptyAfterAdapter(IEnumerable<Location> emptyAfterLocations, JourneyCollection journeyCollection)
    {
        this._emptyAfterLocations = emptyAfterLocations;
        _journeyCollection = journeyCollection;
    }
    public void AttachToTransition(Transition transition)
    {
        foreach (var place in _emptyAfterLocations.Select(e => LocationTranslator.CreatePlace(e, _journeyCollection)))
            transition.AddInhibitorFrom(place, 1);
    }
}