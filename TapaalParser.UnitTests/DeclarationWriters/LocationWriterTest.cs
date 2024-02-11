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
        this.LocationGenerator = new LocationGenerator(Parts, markingStrategy: MarkingStrategy.None);
    }

    public LocationGenerator LocationGenerator { get; set; }

    [Fact]
    public void WhenWritingCapacityLocations_ShouldWriteEquivalentPlaces()
    {
        var locations = LocationGenerator.Generate(10).ToArray();
        var capacityLocations = locations.Select(e => e.ToCapacityLocation());
        var capacityPlaces = locations.Select(loc => PlaceFactory.CreateInitializedCapacityPlaceFor(loc));

        var oldText = OldText([], capacityLocations);        
        var newText = NewText(capacityPlaces);
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