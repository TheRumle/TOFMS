using Tofms.Common;

namespace TACPN.Net;

public static class CapacityPlaceCreator
{
    public static readonly string DefaultColorName = Place.CapacityPlaceColor;
    public static readonly string Hat = "_buffer";

    public static string CapacityNameFor(Place place)
    {
        return place.Name + "_buffer";
    }
    
    public static string CapacityNameFor(string placeName)
    {
        return placeName + "_buffer";
    }

    public static Place CapacityPlaceFor(Place place)
    {
        var dotValue = KeyValuePair.Create(DefaultColorName, (int)InfinityInteger.Positive);
        return Place.CapacityPlace(CapacityNameFor(place));
    }
}