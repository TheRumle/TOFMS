using TACPN.Net;
using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using Tofms.Common;
using Tofms.Common.JsonTofms.Models;

namespace TACPN.Adapters.TofmToTacpnAdapter;

public static class LocationTranslator
{
    public static (Place place, CapacityPlace placeHat) CreatePlaceAndCapacityPlacePair(Location location, JourneyCollection collection)
    {
        var place = CreatePlace(location, collection);
        var placeHat = place.ToCapacityPlace(location.Capacity);
        return (place, placeHat);
    }

    public static Place CreatePlace(Location location, JourneyCollection journeyCollection)
    {
        var colourType = ColourType.TokensColourType;
        var maxAges = location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max));

        List<ColourInvariant<int, string>> invariants =
            CreateInvariants(colourType, location, journeyCollection, new Dictionary<string, int>(maxAges));
        
        Place p = new Place(location.IsProcessing, location.Name, invariants, colourType);
        return p;
    }

    private static List<ColourInvariant<int, string>> CreateInvariants(ColourType colourType, Location location, JourneyCollection journeyCollection, Dictionary<string, int> maxAges)
    {
        List<ColourInvariant<int, string>> result = new List<ColourInvariant<int, string>>(); 
        foreach (var l in journeyCollection)
        {
            var partName = l.Key;
            var journey = l.Value;
            var partIndexPair = journey.Where(e => e.Value.Name == location.Name);
            foreach (var kvp in partIndexPair)
            {
                int index = kvp.Key;
                var maxAge = maxAges[partName];
                ColourInvariant<int, string> inv =
                    new ColourInvariant<int, string>(colourType, index, partName, maxAge);
                
                result.Add(inv);
            }

        }

        return result;
    }
}