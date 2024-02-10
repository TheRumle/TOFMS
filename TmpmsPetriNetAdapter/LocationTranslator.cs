using TACPN.Colours;
using TACPN.Colours.Type;
using TACPN.Places;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter;



public class PlaceFactory
{
    public const string CAPACITY_PLACE_POSTFIX = "_capacity";
    private readonly ColourType PartsColour;
    private readonly IndexedJourneyCollection IndexedJourneyCollection;
    
    public PlaceFactory(ColourTypeFactory factory, IndexedJourneyCollection journeyCollection)
    {
        PartsColour = factory.Parts;
        this.ColourTypeFactory = factory;
        this.IndexedJourneyCollection = journeyCollection;
    }

    public ColourTypeFactory ColourTypeFactory { get; set; }

    public Place CreatePlace(Location location) => CreatePlace(location, IndexedJourneyCollection, PartsColour);
    
    public static Place CreatePlace(Location location, IndexedJourneyCollection indexedJourneyCollection, ColourType colourType)
    {
        var maxAges = location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max));
        var invariants = 
            CreateInvariants(colourType, location, indexedJourneyCollection, new Dictionary<string, int>(maxAges));
        
        return new Place(location.Name, invariants, colourType);
    }
    private static List<ColourInvariant<string>> CreateInvariants(ColourType colourType, Location location, IndexedJourneyCollection indexedJourneyCollection, Dictionary<string, int> maxAges)
    {
        List<ColourInvariant<string>> result = new List<ColourInvariant<string>>(); 
        foreach (var l in indexedJourneyCollection)
        {
            var partName = l.Key;
            var journey = l.Value;
            var partIndexPair = journey.Where(e => e.Value.Name == location.Name);
            foreach (var kvp in partIndexPair)
            {
                int index = kvp.Key;
                var maxAge = maxAges[partName];
                ColourInvariant<string> inv = new ColourInvariant<string>(colourType, index.ToString(), maxAge);
                result.Add(inv);
            }

        }

        return result;
    }

    public (Place place, Place capacityPlace) CreatePlaceAndCapacityPlacePair(Location location)
    {
        var place = CreatePlace(location);
        var capacityPlace = CreateCapacityPlaceFor(location);
        return (place, capacityPlace);
    }

    public Place CreateCapacityPlaceFor(Location location)
    {
        return new Place(location.Name + CAPACITY_PLACE_POSTFIX, [ColourInvariant.DotDefault], ColourTypeFactory.DotColour)
        {
            IsCapacityLocation = true
        };
    }
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

        var invariants =
            CreateInvariants(colourType, location, indexedJourneyCollection, new Dictionary<string, int>(maxAges));
        
        return new Place(location.Name, invariants, colourType);
    }
    
    public static CapacityPlace CreateCapacityPlace(Location location)
    {
        return new CapacityPlace(location.CapacityName(),location.Capacity);
    }

    private static IEnumerable<ColourInvariant<string>> CreateInvariants(ColourType colourType, Location location, IndexedJourneyCollection indexedJourneyCollection, Dictionary<string, int> maxAges)
    {
        foreach (var l in indexedJourneyCollection)
        {
            var partIndexPair = l.Value.Where(e => e.Value.Name == location.Name);
            foreach (var kvp in partIndexPair)
            {
                int index = kvp.Key;
                var maxAge = maxAges[l.Key];
                yield return new ColourInvariant<string>(colourType, index.ToString(), maxAge);
            }
        }
    }
}