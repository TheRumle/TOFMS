using System.Collections;
using System.Text;
using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Places;
using TapaalParser.TapaalGui.Writers;
using TestDataGenerator;
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
    public void WhenWritingCapacityLocations_ShouldWriteEquivalentPlaces()
    {
        var locations = Journeys.Locations.ToArray();
        var capacityLocations = locations.Select(e => e.ToCapacityLocation());
        var capacityPlaces = locations.Select(loc => PlaceFactory.CreateInitializedCapacityPlaceFor(loc));

        var oldText = OldText([], capacityLocations);        
        var newText = NewText(capacityPlaces);
        newText.Should().Be(oldText);
    }
    
    [Theory]
    [InlineData(ProcessingLocationStrategy.OnlyProcessingLocations)]
    [InlineData(ProcessingLocationStrategy.OnlyRegularLocations)]
    public void WhenWritingNonCapacityLocations_ShouldWriteEquivalentPlaces(ProcessingLocationStrategy strategy)
    {
        var journey = CreateJourney(new LocationGenerator(Parts, processingLocationStrategy: strategy));
        
        var locations = journey.SelectMany(e=>e.Value).ToArray();
        var places = locations.Select(loc => PlaceFactory.CreatePlace(loc));

        var oldText = OldText(locations, []);        
        var newText = NewText(places);
        
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