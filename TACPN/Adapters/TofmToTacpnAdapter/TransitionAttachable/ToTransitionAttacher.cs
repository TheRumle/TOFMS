using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;

internal class ToTransitionAttacher : ITransitionAttachable
{
    public ToTransitionAttacher(Location location,
        IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace, JourneyCollection journeyCollection)
    {
        _itemMovedIntoPlace = partsItemMovedIntoPlace;
        (_place, _capacityPlace) = LocationTranslator.CreatePlaceAndCapacityPlacePair(location, journeyCollection);
    }

    private readonly CapacityPlace _capacityPlace;

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
        transition.AddInGoingFrom(_capacityPlace, consumationAmount);
    }

    private void AdaptPlace(Transition transition)
    {
        IEnumerable<Production> productions = _itemMovedIntoPlace.Select(e => new Production(_place.ColourType, e.Value));
        transition.AddOutGoingTo(_place, productions);
    }
}