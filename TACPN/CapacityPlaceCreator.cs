using Common;
using TACPN.Net;

namespace TACPN;

public static class CapacityPlaceCreator
{

    public static readonly string DefaultColorName = "dot";
    public static string CapacityNameFor(Place place)
    {
        return place.Name + "_buffer";
    }
    
    public static string CapacityNameFor(string name)
    {
        return name + "_buffer";
    }
    
    public static Place CapacityPlaceFor(Place place)
    {
        var infinityPlaces = place.ColorInvariants.Select(kvp => 
            new KeyValuePair<string, int>(DefaultColorName, InfinityInteger.Positive));
        return new Place(CapacityNameFor(place), infinityPlaces);
    }
}