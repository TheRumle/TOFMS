using TACPN.Colours;
using TACPN.Colours.Type;
using TACPN.Places;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter;



public class PlaceFactory
{
    public PlaceFactory(ColourTypeFactory factory, IndexedJourneyCollection journeyCollection)
    {
        PartsColour = factory.Parts;
        this.IndexedJourneyCollection = journeyCollection;
    }

    public readonly ColourType PartsColour;
    public readonly IndexedJourneyCollection IndexedJourneyCollection;
    public Place CreatePlace(Location location) => LocationTranslator.CreatePlace(location, IndexedJourneyCollection, PartsColour);
}

public static class LocationTranslator
{
    public static (Place place, CapacityPlace placeHat) CreatePlaceAndCapacityPlacePair(Location location, IndexedJourneyCollection collection, ColourType ct)
    {
        var place = CreatePlace(location, collection, ct);
        var placeHat = place.ToCapacityPlace(location.Capacity);
        return (place, placeHat);
    }

    public static Place CreatePlace(Location location, IndexedJourneyCollection indexedJourneyCollection, ColourType colourType)
    {
        var maxAges = location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max));

        List<ColourInvariant<int, string>> invariants =
            CreateInvariants(colourType, location, indexedJourneyCollection, new Dictionary<string, int>(maxAges));
        
        Place p = new Place(location.IsProcessing, location.Name, invariants, colourType);
        return p;
    }
    
    public static CapacityPlace CreateCapacityPlace(Location location)
    {
        return new CapacityPlace(location.CapacityName(), location.Capacity);
    }

    private static List<ColourInvariant<int, string>> CreateInvariants(ColourType colourType, Location location, IndexedJourneyCollection indexedJourneyCollection, Dictionary<string, int> maxAges)
    {
        List<ColourInvariant<int, string>> result = new List<ColourInvariant<int, string>>(); 
        foreach (var l in indexedJourneyCollection)
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