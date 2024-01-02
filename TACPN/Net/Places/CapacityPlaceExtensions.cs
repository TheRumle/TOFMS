namespace TACPN.Net.Places;

public static class CapacityPlaceExtensions
{
    public static readonly string DefaultCapacityColor = "dot";
    public static readonly string Hat = "_capacity";

    private static string CapacityNameFor(Places.Place place)
    {
        return place.Name + Hat;
    }
    
    public static string CapacityNameFor(string placeName)
    {
        return placeName + Hat;
    }
    
    
    public static CapacityPlace ToCapacityPlace(this Places.Place place, int numTokens)
    {
        return  new CapacityPlace(CapacityNameFor(place), numTokens);
    }
}