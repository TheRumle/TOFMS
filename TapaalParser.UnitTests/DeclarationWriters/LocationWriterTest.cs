using System.Collections;
using System.Text;
using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Places;
using TapaalParser.TapaalGui.Writers;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter;
using Xml;

namespace TaapalParser.UnitTests.DeclarationWriters;

public class LocationWriterTest : WriterTest
{
    public readonly PlaceFactory PlaceFactory;

    public LocationWriterTest(MoveActionFixture fixture) : base(fixture)
    {
        this.PlaceFactory = new PlaceFactory(ColourFactory, Journeys.ToIndexedJourney());
    }

    [Fact]
    public void ShouldWriteSame()
    {

        var allLocations = Journeys.SelectMany(e => e.Value).ToArray();


        var movinglocations = allLocations.Where(e => e.IsProcessing).ToArray();
        var processingLocations = allLocations.Where(e => !e.IsProcessing).ToArray();
        
        
        
        
        
        
        
        var capacityPlaces = movinglocations.Select(e => PlaceFactory.CreateInitializedCapacityPlaceFor(e));
        var places = movinglocations.Select(e => PlaceFactory.CreatePlace(e));

        var oldText = OldText(movinglocations, processingLocations.Select(e=>e.ToCapacityLocation()));
        
        
        
        
        var newText = NewText([..places, ..capacityPlaces]);

        newText.Should().Be(oldText);
    }

    private string OldText(IEnumerable<Location> locations, IEnumerable<CapacityLocation> capacityLocations)
    {
        var indexed = Journeys.ToIndexedJourney();
        SharedPlaceDeclarationWriter declarationWriter = new SharedPlaceDeclarationWriter(new StringBuilder());
        declarationWriter.WritePlaces(locations, indexed);
        declarationWriter.WriteCapacityPlaces( capacityLocations, indexed);
        return RemoveWhiteSpace(declarationWriter.StringBuilder.ToString());
    }

    private string NewText(IEnumerable<Place> locations)
    {
        var writer = new LocationWriter(locations);
        writer.AppendAllText();
        return RemoveWhiteSpace(writer.ToString());
    }
}