using TACPN.Colours.Values;
using TACPN.Places;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter;

public static class PlaceExtensions
{

    public static bool IsCreatedFrom(this IPlace place, Location location)
    {
        //If the place is a capacityLocation we can only compare the bufferized name of the location and the name of the place.
        if (place.IsCapacityLocation)
            return place.Name == CapacityNameFor(location.Name);
        var nameMatches = place.Name == location.Name;
        return nameMatches;
    }
    public static readonly string Hat = "_capacity";

    public static string CapacityNameFor(this Place place)
    {
        return place.Name + Hat;
    }
    
    public static string CapacityName(this Location place)
    {
        return place.Name + Hat;
    }
    
    public static string CapacityNameFor(string placeName)
    {
        return placeName + Hat;
    }
    
    
    public static CapacityPlace ToCapacityPlace(this Place place, int numTokens)
    {
        return  new CapacityPlace(CapacityNameFor(place), numTokens);
    }

}