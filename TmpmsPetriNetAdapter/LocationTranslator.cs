using TACPN.Colours;
using TACPN.Colours.Type;
using TACPN.Places;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter;

public static class LocationTranslator
{
    public static (Place place, CapacityPlace placeHat) CreatePlaceAndCapacityPlacePair(Location location, IndexedJourney collection)
    {
        var place = CreatePlace(location, collection);
        var placeHat = place.ToCapacityPlace(location.Capacity);
        return (place, placeHat);
    }

    public static Place CreatePlace(Location location, IndexedJourney indexedJourney)
    {
        var colourType = ColourType.TokensColourType;
        var maxAges = location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max));

        List<ColourInvariant<int, string>> invariants =
            CreateInvariants(colourType, location, indexedJourney, new Dictionary<string, int>(maxAges));
        
        Place p = new Place(location.IsProcessing, location.Name, invariants, colourType);
        return p;
    }
    
    public static CapacityPlace CreateCapacityPlacePlace(Location location, IndexedJourney collection)
    {
        var place = CreatePlace(location, collection);
        var placeHat = place.ToCapacityPlace(location.Capacity);
        return placeHat;
    }

    private static List<ColourInvariant<int, string>> CreateInvariants(ColourType colourType, Location location, IndexedJourney indexedJourney, Dictionary<string, int> maxAges)
    {
        List<ColourInvariant<int, string>> result = new List<ColourInvariant<int, string>>(); 
        foreach (var l in indexedJourney)
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