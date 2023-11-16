using Common;
using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN;

internal class FromLocationAdaption : ITransitionAdaptable
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="place"> A place representing a location l.</param>
    /// <param name="capacityPlace">The corresponding place enforcing the capacity of the location l</param>
    public FromLocationAdaption(Location location, IEnumerable<KeyValuePair<string, int>> partsToConsume)
    {
        this.Location = location;
        this.ToConsume = partsToConsume;
        (Place, CapacityPlace) = LocationTranslator.CreateLocationPlacePair(location);
    }

    public Location Location { get; set; }

    public IEnumerable<KeyValuePair<string, int>> ToConsume { get; set; }

    public Place CapacityPlace { get; set; }

    public Place Place { get; set; }

    public void AdaptToTransition(Transition transition)
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