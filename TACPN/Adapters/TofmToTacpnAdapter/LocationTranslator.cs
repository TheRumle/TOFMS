using TACPN.Net;
using Tofms.Common;
using Tofms.Common.JsonTofms.Models;

namespace TACPN.Adapters.TofmToTacpnAdapter;

public static class LocationTranslator
{
    public static (Place place, Place placeHat) CreatePlaceAndCapacityPlacePair(Location location)
    {
        var place = CreatePlace(location);
        var placeHat = place.ToCapacityPlace(location.Capacity);
        return (place, placeHat);
    }

    public static Place CreatePlace(Location location)
    {
        var colourType = new ColourType(TofmSystem.PRODUCTNAME, location.Invariants.Select(e => e.PartType));
        return new Place(location.Name,
            location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max)),colourType);
    }
}