using TACPN.Net;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.LocationAdapters;

public static class PlaceExtensions
{

    public static bool IsCreatedFrom(this Place place, Location location)
    {
        //If the place is a capacityLocation we can only compare the bufferized name of the location and the name of the place.
        if (place.IsCapacityLocation)
            return place.Name == CapacityPlaceCreator.CapacityNameFor(location.Name);

        //Create ValueTuples that are structurally equivalent
        var parts = location.Invariants.Select(e => ValueTuple.Create(e.PartType,e.Max))
            .ToHashSet();
        
        var colors = place.ColorInvariants.Select(e => ValueTuple.Create(e.Key,e.Value))
            .ToHashSet();
        
        //Do they have same name and are based on same invariants
        var nameMatches = place.Name == location.Name;
        return nameMatches && parts.SetEquals(colors);
    }

}