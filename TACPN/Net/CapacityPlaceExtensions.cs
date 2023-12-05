using Tofms.Common;

namespace TACPN.Net;

public static class CapacityPlaceExtensions
{
    public static readonly string DefaultCapacityColor = "dot";
    public static readonly string Hat = "_capacity";

    private static string CapacityNameFor(Place place)
    {
        return place.Name + Hat;
    }
    
    public static string CapacityNameFor(string placeName)
    {
        return placeName + Hat;
    }
    
    
    public static CapacityPlace ToCapacityPlace(this Place place, int numTokens)
    {
        var kvp = KeyValuePair.Create(DefaultCapacityColor, (int)InfinityInteger.Positive);
        return  new CapacityPlace(CapacityNameFor(place), numTokens);
    }
}