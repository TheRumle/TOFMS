using Bogus;
using TACPN.Places;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter;
using TmpmsPetriNetAdapter.Colours;

namespace TestDataGenerator;

public class PlaceGenerator : Generator<Place>
{
    private readonly IndexedJourneyCollection _collection;
    private readonly ColourTypeFactory _ctFactory;
    private readonly LocationGenerator _locationGenerator;
    private readonly PlaceFactory _placeFactory;

    public PlaceGenerator(ColourTypeFactory ctFactory, JourneyCollection collection, Faker<Place> faker) : base(faker)
    {
        _ctFactory = ctFactory;
        _locationGenerator = new LocationGenerator(ctFactory.Parts.Colours.Select(e => e.Value));
        this._placeFactory = new PlaceFactory(_ctFactory, collection);
    }

    public override Place GenerateSingle()
    {
        return this._placeFactory.CreatePlace(_locationGenerator.GenerateSingle());
    }

    public CapacityPlace GenerateCapacityPlace()
    {
        var location = _locationGenerator.GenerateSingle();
        var created = this._placeFactory.CreatePlace(location);
        return new CapacityPlace(created.Name, location.Capacity);
    }
}