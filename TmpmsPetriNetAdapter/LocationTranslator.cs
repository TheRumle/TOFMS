using TACPN.Colours;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
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
    
    public Place CreatePlace(Location location, IndexedJourneyCollection indexedJourneyCollection, ColourType colourType)
    {
        var maxAges = location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max));
        var invariants = 
            CreateInvariants(colourType, location, indexedJourneyCollection, new Dictionary<string, int>(maxAges));
        
        return new Place(location.Name, invariants, colourType);
    }
    private List<ColourInvariant> CreateInvariants(ColourType colourType, Location location, IndexedJourneyCollection indexedJourneyCollection, Dictionary<string, int> maxAges)
    {
        List<ColourInvariant> result = new List<ColourInvariant>(); 
        foreach (var l in indexedJourneyCollection)
        {
            var partName = l.Key;
            var journey = l.Value;
            var partIndexPair = journey.Where(e => e.Value.Name == location.Name);
            foreach (var kvp in partIndexPair)
            {
                int index = kvp.Key;
                var maxAge = maxAges[partName];
                ColourInvariant inv = new ColourInvariant(colourType, new TupleColour([new Colour(partName) , new ColourIntValue(index)], ColourTypeFactory.Tokens), maxAge);
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
