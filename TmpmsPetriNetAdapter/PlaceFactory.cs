using TACPN;
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
    private readonly ColourType _partsColour;
    private readonly IndexedJourneyCollection _indexedJourneyCollection;
    private readonly ColourTypeFactory _colourTypeFactory;

    
    public PlaceFactory(ColourTypeFactory factory, IndexedJourneyCollection journeyCollection)
    {
        _partsColour = factory.Parts;
        this._colourTypeFactory = factory;
        this._indexedJourneyCollection = journeyCollection;
    }

    public Place CreatePlace(Location location) => CreatePlace(location, _indexedJourneyCollection, _colourTypeFactory.Tokens);
    
    public Place CreatePlace(Location location, IndexedJourneyCollection indexedJourneyCollection, ColourType colourType)
    {
        var maxAges = location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max));
        var invariants =  CreateInvariants(colourType, location, indexedJourneyCollection, new Dictionary<string, int>(maxAges));
        
        return new Place(location.Name, invariants, colourType);
    }
    private List<ColourInvariant> CreateInvariants(ColourType colourType, Location location, IndexedJourneyCollection indexedJourneyCollection, Dictionary<string, int> maxAges)
    {
        var createInvariant = (string partName, int index) => new ColourInvariant(colourType, new TupleColour([new Colour(partName), new ColourIntValue(index)],
            _colourTypeFactory.Tokens), maxAges[partName]);
        
        List<ColourInvariant> result = []; 
        foreach (var (partName, journey) in indexedJourneyCollection)
        {
            var jour = journey.Where(e => e.Value.Name == location.Name);
            result.AddRange(jour
                .Select(kvp => createInvariant.Invoke(partName, kvp.Key)));
        }

        return result;
    }

    public (Place place, Place capacityPlace) CreatePlaceAndCapacityPlacePair(Location location)
    {
        var place = CreatePlace(location);
        var capacityPlace = CreateInitializedCapacityPlaceFor(location);
        return (place, capacityPlace);
    }

    
    /// <summary>
    /// Creates a capacity place representing remaining capacity. Initializes the place with marking of 'dot' tokens equal to remaining capacity.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    
    public Place CreateInitializedCapacityPlaceFor(Location location)
    {
        var capacityPlace =  new Place(location.Name + CAPACITY_PLACE_POSTFIX, [ColourInvariant.DotDefault], ColourTypeFactory.DotColour)
        {
            IsCapacityLocation = true
        };

        var capacityLeft = location.Capacity - location.Configuration.Size;
        capacityPlace.AddTokenOfColour(Enumerable.Repeat(Colour.DefaultTokenColour, capacityLeft));
        return capacityPlace;
    }
}
