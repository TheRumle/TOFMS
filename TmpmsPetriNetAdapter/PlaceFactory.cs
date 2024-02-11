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
    private readonly JourneyCollection _journeys;
    private readonly ColourTypeFactory _colourTypeFactory;

    
    public PlaceFactory(ColourTypeFactory factory, JourneyCollection collection)
    {
        this._colourTypeFactory = factory;
        this._journeys = collection;
    }

    public Place CreatePlace(Location location) => CreatePlace(location, _journeys, _colourTypeFactory.Tokens);
    
    public Place CreatePlace(Location location, JourneyCollection journeys, ColourType colourType)
    {
        var maxAges = location.Invariants.Select(e =>(e.PartType, e.Max));
        var invariants = CreateInvariants(location, journeys, maxAges);
        return new Place(location.Name, invariants, colourType);
    }

    private IEnumerable<ColourInvariant> CreateInvariants(Location location, JourneyCollection journeys, IEnumerable<(string partType, int maxAge)> maxAges)
    {
        foreach (var (partType, maxAge) in maxAges)
        {
            var indexes = journeys.GetOccurrencesFor(partType, location);
            foreach (var index in indexes)
            {
                var colour = new TupleColour([new Colour(partType), new ColourIntValue(index)], _colourTypeFactory.Tokens);
                yield return new ColourInvariant(_colourTypeFactory.Tokens, colour, maxAge);
            }
        }
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
