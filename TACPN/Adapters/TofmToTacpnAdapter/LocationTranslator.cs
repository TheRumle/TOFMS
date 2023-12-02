using TACPN.Net;
using Tofms.Common;
using Tofms.Common.JsonTofms.Models;

namespace TACPN.Adapters.TofmToTacpnAdapter;

public static class LocationTranslator
{
    private const string _partColours = TofmSystem.PRODUCTNAME;
    public static (Place place, Place placeHat) CreateLocationPlacePair(Location location)
    {
        var place = CreatePlace(location);
        var placeHat = place.ToCapacityPlace(location.Capacity);
        return (place, placeHat);
    }

    public static Place CreatePlace(Location location)
    {
        return new Place(location.Name,
            location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max)),_partColours );
    }
}