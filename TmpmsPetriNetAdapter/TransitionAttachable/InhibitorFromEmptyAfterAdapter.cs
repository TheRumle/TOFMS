using TACPN.Transitions;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class InhibitorFromEmptyAfterAdapter : ITransitionAttachable
{
    private readonly IEnumerable<Location> _emptyAfterLocations;
    private readonly IndexedJourney _indexedJourney;

    public InhibitorFromEmptyAfterAdapter(IEnumerable<Location> emptyAfterLocations, IndexedJourney indexedJourney)
    {
        this._emptyAfterLocations = emptyAfterLocations;
        _indexedJourney = indexedJourney;
    }
    public void AttachToTransition(Transition transition)
    {
        foreach (var place in _emptyAfterLocations.Select(e => LocationTranslator.CreatePlace(e, _indexedJourney)))
            transition.AddInhibitorFrom(place, 1);
    }
}