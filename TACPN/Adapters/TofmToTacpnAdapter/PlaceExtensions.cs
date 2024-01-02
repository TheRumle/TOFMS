using TACPN.Net;
using TACPN.Net.Places;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter;

public static class PlaceExtensions
{

    public static bool IsCreatedFrom(this IPlace place, Location location)
    {
        //If the place is a capacityLocation we can only compare the bufferized name of the location and the name of the place.
        if (place.IsCapacityLocation)
            return place.Name == CapacityPlaceExtensions.CapacityNameFor(location.Name);
        var nameMatches = place.Name == location.Name;
        return nameMatches;
    }

}