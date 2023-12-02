using Tofms.Common;

namespace TACPN.Net;

public static class CapacityPlaceExtensions
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
    
    
    public static Place ToCapacityPlace(this Place place, int numTokens)
    {
        var kvp = KeyValuePair.Create(DefaultColorName, (int)InfinityInteger.Positive);
        return new Place(CapacityNameFor(place), new[] { kvp })
        {
            IsCapacityLocation = true,
            Tokens = new TokenCollection(Enumerable.Repeat(new Token(DefaultColorName), numTokens))
            {
                Colours = new []{DefaultColorName}
            }
        };
    }
}