using Common;
using TACPN.Net;

namespace TACPN;

public class LocationTranslator
{
    public static (Place to, Place toHat) CreateLocationPlacePair(Location location)
    {
        Place to = new Place(location.Name,
            location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max)));
        
        Place toHat = CapacityPlaceCreator.CapacityPlaceFor(to);
        return (to, toHat);
    }

}