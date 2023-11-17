using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;

namespace TACPN.Adapters.Location;

internal class ToTransitionAttacher : ITransitionAttachable
{
    public ToTransitionAttacher(Common.Location location,
        IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace)
    {
        Location = location;
        ItemMovedIntoPlace = partsItemMovedIntoPlace;
        (Place, CapacityPlace) = LocationTranslator.CreateLocationPlacePair(location);
    }

    public Place CapacityPlace { get; set; }
    public Place Place { get; set; }

    public IEnumerable<KeyValuePair<string, int>> ItemMovedIntoPlace { get; set; }

    public Common.Location Location { get; set; }


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
        var consumationAmount = ItemMovedIntoPlace.Sum(e => e.Value);
        var guard = ColoredGuard.CapacityGuard(consumationAmount);
        transition.AddInGoingFrom(CapacityPlace, guard);
    }

    private void AdaptPlace(Transition transition)
    {
        IEnumerable<Production> productions = ItemMovedIntoPlace.Select(e => new Production(e.Key, e.Value));
        transition.AddOutGoingTo(Place, productions);
    }
}