using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;

namespace TACPN.Adapters.Location;

internal class FromLocationAdaption : ITransitionAttachable
{
    /// <summary>
    /// </summary>
    /// <param name="place"> A place representing a location l.</param>
    /// <param name="capacityPlace">The corresponding place enforcing the capacity of the location l</param>
    public FromLocationAdaption(Common.Location location, IEnumerable<KeyValuePair<string, int>> partsToConsume)
    {
        Location = location;
        ToConsume = partsToConsume;
        (Place, CapacityPlace) = LocationTranslator.CreateLocationPlacePair(location);
    }

    public Common.Location Location { get; set; }

    public IEnumerable<KeyValuePair<string, int>> ToConsume { get; set; }

    public Place CapacityPlace { get; set; }

    public Place Place { get; set; }

    public void AttachToTransition(Transition transition)
    {
        AdaptPlace(transition);
        AdaptCapacityPlace(transition);
    }

    private void AdaptCapacityPlace(Transition transition)
    {
        var consProdAmount = ToConsume.Sum(e => e.Value);
        var capPlaceColor = CapacityPlaceCreator.DefaultColorName;
        transition.AddOutGoingTo(CapacityPlace, new Production(capPlaceColor, consProdAmount));
        transition.AddInGoingFrom(CapacityPlace,
            new ColoredGuard(consProdAmount, capPlaceColor, Interval.ZeroToInfinity));
    }

    private void AdaptPlace(Transition transition)
    {
        var guards = ToConsume.Select(pair =>
        {
            var first = Location.Invariants.First(e => e.PartType == pair.Key);
            return new ColoredGuard(pair.Value, pair.Key, new Interval(first.Min, first.Max));
        });

        transition.AddInGoingFrom(Place, guards);
    }
}