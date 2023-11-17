using TACPN.Net;

namespace TACPN.Adapters.Location;

public static class LocationTranslator
{
    public static (Place place, Place placeHat) CreateLocationPlacePair(Common.Location location)
    {
        var place = CreateLocation(location);
        var placeHat = CapacityPlaceCreator.CapacityPlaceFor(place);
        return (place, placeHat);
    }

    public static Place CreateLocation(Common.Location location)
    {
        return new Place(location.Name,
            location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max)));
    }
}