using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;

internal class ToTransitionAttacher : ITransitionAttachable
{
    public ToTransitionAttacher(Tofms.Common.Location location,
        IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace)
    {
        _itemMovedIntoPlace = partsItemMovedIntoPlace;
        (_place, _capacityPlace) = LocationTranslator.CreateLocationPlacePair(location);
    }

    private readonly Place _capacityPlace;

    private readonly Place _place;

    private readonly IEnumerable<KeyValuePair<string, int>> _itemMovedIntoPlace;


    public void AttachToTransition(Transition transition)
    {
        AdaptPlace(transition);
        AdaptCapacityPlace(transition);
    }

    /// <summary>
    /// Add ingoing for with dots amount of any age for each thing we move
    /// </summary>
    /// <param name="transition"></param>
    private void AdaptCapacityPlace(Transition transition)
    {
        var consumationAmount = _itemMovedIntoPlace.Sum(e => e.Value);
        var guard = ColoredGuard.CapacityGuard(consumationAmount);
        transition.AddInGoingFrom(_capacityPlace, guard);
    }

    private void AdaptPlace(Transition transition)
    {
        IEnumerable<Production> productions = _itemMovedIntoPlace.Select(e => new Production(e.Key, e.Value));
        transition.AddOutGoingTo(_place, productions);
    }
}