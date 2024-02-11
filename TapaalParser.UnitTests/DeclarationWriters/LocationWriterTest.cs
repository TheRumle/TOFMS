using System.Text;
using FluentAssertions;
using TACPN.Places;
using TapaalParser.TapaalGui.Writers;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter;
using TmpmsPetriNetAdapter.Colours;
using Xml;

namespace TaapalParser.UnitTests.DeclarationWriters;

public class LocationWriterTest : NoWhitespaceWriterTest
{
    private IEnumerable<string> Parts = ["P1", "P2", "P3"];

    public PlaceFactory CreatePlaceFactory(JourneyCollection collection)
    {
        return new PlaceFactory(new ColourTypeFactory(Parts,collection), collection);
    }
    
    public JourneyCollection CreateJourney(LocationGenerator generator)
    {
        var journeys = Parts.Select(e => (e, generator.Generate(new Random().Next(2,6))));
        return JourneyCollection.ConstructJourneysFor(journeys.ToArray());
    }

    [Fact]
    public void WhenWritingCapacityLocations_ShouldWriteEquivalentPlaces()
    {
        var journeys = CreateJourney(new LocationGenerator(Parts));
        var placeFactory = CreatePlaceFactory(journeys);
        var locations = journeys.Locations.ToArray();
        
        var capacityLocations = locations.Select(e => e.ToCapacityLocation());
        var capacityPlaces = locations.Select(loc => placeFactory.CreateInitializedCapacityPlaceFor(loc));

        var oldText = OldText(journeys,[], capacityLocations);        
        var newText = NewText(capacityPlaces);
        newText.Should().Be(oldText);
    }
    
    [Theory]
    [InlineData(ProcessingLocationStrategy.OnlyProcessingLocations)]
    [InlineData(ProcessingLocationStrategy.OnlyRegularLocations)]
    public void WhenWritingNonCapacityLocations_ShouldWriteEquivalentPlaces(ProcessingLocationStrategy strategy)
    {
        var journey = CreateJourney(new LocationGenerator(Parts, processingLocationStrategy: strategy));
        var placeFactory = CreatePlaceFactory(journey);
        
        var locations = journey.SelectMany(e=>e.Value).ToArray();
        var places = locations.Select(loc => placeFactory.CreatePlace(loc));

        var oldText = OldText(journey, locations, []);        
        var newText = NewText(places);
        
        newText.Should().Be(oldText);
    }

    private string OldText(JourneyCollection journeyCollection, IEnumerable<Location> locations, IEnumerable<CapacityLocation> capacityLocations)
    {
        var indexed = journeyCollection.ToIndexedJourney();
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