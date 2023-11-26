using TACPN.Net;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.LocationAdapters;

public static class LocationTranslator
{
    public static (Place place, Place placeHat) CreateLocationPlacePair(Location location)
    {
        var place = CreateLocation(location);
        var placeHat = CapacityPlaceCreator.CapacityPlaceFor(place);
        return (place, placeHat);
    }

    public static Place CreateLocation(Location location)
    {
        return new Place(location.Name,
            location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max)));
    }
}