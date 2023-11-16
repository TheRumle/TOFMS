using Common;
using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN;

internal class FromAdaption : ITransitionAdaptable
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="place"> A place representing a location l.</param>
    /// <param name="capacityPlace">The corresponding place enforcing the capacity of the location l</param>
    public FromAdaption(Location location, IEnumerable<KeyValuePair<string, int>> partsToConsume)
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
        var Guards = new List<ColoredGuard>();
        foreach (var pair in ToConsume)
        {
            var color = pair.Key;
            var amount = pair.Value;
            var first = Location.Invariants.First(e => e.PartType == color);
            Guards.Add(new ColoredGuard(amount, color,new Interval(first.Min, first.Max)));
        }

        transition.AddInGoingFrom(Place, Guards);
        
        throw new NotImplementedException();
    }
}