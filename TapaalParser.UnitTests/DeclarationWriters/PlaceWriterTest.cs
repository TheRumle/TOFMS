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

public class PlaceWriterTest : NoWhitespaceWriterTest
{
    private static IEnumerable<string> Parts = ["P1", "P2", "P3"];
    private LocationGenerator _generator = new(Parts);

    public PlaceFactory CreatePlaceFactory(JourneyCollection collection)
    {
        return new PlaceFactory(new ColourTypeFactory(Parts,collection));
    }
    
    public (Location[] nonProcessing, JourneyCollection journey, Location[] allLocations) CreateJourneyAndLocations()
    {
        var otherLocations = _generator.Generate(new Random().Next(2, 6), ProcessingLocationStrategy.OnlyRegularLocations).ToArray();
        var journeySequence = Parts.Select(e => (e, _generator.Generate(new Random().Next(2, 6), ProcessingLocationStrategy.OnlyProcessingLocations)));
        var journey = JourneyCollection.ConstructJourneysFor(journeySequence.ToArray());

        var allLocations = journey.SelectMany(e => e.Value)
            .Union(otherLocations).ToArray();
        
        return (otherLocations, journey, allLocations);
    }
    
    
    private string OldCapacityPlaceText(JourneyCollection journeyCollection, IEnumerable<CapacityLocation> capacityLocations)
    {
        var indexed = journeyCollection.ToIndexedJourney();
        SharedPlaceDeclarationWriter declarationWriter = new SharedPlaceDeclarationWriter(new StringBuilder());
        declarationWriter.WriteCapacityPlaces( capacityLocations, indexed);
        return RemoveWhiteSpace(declarationWriter.StringBuilder.ToString());
    }
    
    private string OldTranslationText(JourneyCollection journeyCollection, IEnumerable<Location> locations, IEnumerable<CapacityLocation> capacityLocations)
    {
        var indexed = journeyCollection.ToIndexedJourney();
        SharedPlaceDeclarationWriter declarationWriter = new SharedPlaceDeclarationWriter(new StringBuilder());
        declarationWriter.WriteCapacityPlaces( capacityLocations, indexed);
        declarationWriter.WritePlaces(locations, indexed);
        return RemoveWhiteSpace(declarationWriter.StringBuilder.ToString());
    }
    
    private string OldPlaceText(JourneyCollection journeyCollection, IEnumerable<Location> locations)
    {
        var indexed = journeyCollection.ToIndexedJourney();
        SharedPlaceDeclarationWriter declarationWriter = new SharedPlaceDeclarationWriter(new StringBuilder());
        declarationWriter.WritePlaces(locations, indexed);
        return RemoveWhiteSpace(declarationWriter.StringBuilder.ToString());
    }

    private string NewText(IEnumerable<Place> locations)
    {
        var writer = new PlaceWriter(locations);
        writer.AppendAllText();
        return RemoveWhiteSpace(writer.ToString());
    }
    

    [Fact]
    public void WhenWritingCapacityLocations_ShouldWriteEquivalentPlaceText()
    {
        var (_,journeys,all) = CreateJourneyAndLocations();
        var placeFactory = CreatePlaceFactory(journeys);
        
        var capacityLocations = all.Select(e => e.ToCapacityLocation());
        var capacityPlaces = all.Select(loc => placeFactory.CreateInitializedCapacityPlaceFor(loc));

        var oldText = OldCapacityPlaceText(journeys, capacityLocations);        
        var newText = NewText(capacityPlaces);
        newText.Should().Be(oldText);
    }
    
    [Fact (Skip = "Originally we only create 'infinity places' for non-processing locations. We are more detailed now.")]
    public void WhenWritingNonProcessingLocationsShould_WriteEquivalent()
    {
        var (locations, journey, _) = CreateJourneyAndLocations();
        var placeFactory = CreatePlaceFactory(journey);
        
        
        var places = locations.Select(loc => placeFactory.CreatePlace(loc));

        var oldText = OldPlaceText(journey, locations);        
        var newText = NewText(places);
        
        newText.Should().Be(oldText);
    }
    
    [Fact]
    public void WhenWritingProcessingLocationsShould_WriteEquivalent()
    {
        var (_, journey, allLocations) = CreateJourneyAndLocations();
        var placeFactory = CreatePlaceFactory(journey);
        var processingLocations = allLocations
            .Where(e => e.IsProcessing).ToArray();

        
        var places = processingLocations
            .Select(loc => placeFactory.CreatePlace(loc));

        var oldText = OldPlaceText(journey, processingLocations);        
        var newText = NewText(places);
        
        newText.Should().Be(oldText);
    }
    
    [Fact]
    public void WhenWritingSingleLocationsShould_WriteEquivalent()
    {
        var generator =  new LocationGenerator(["P1"]);
        var target = generator.GenerateSingle(ProcessingLocationStrategy.OnlyRegularLocations) 
            with {Invariants = new HashSet<Invariant>
            {
                Invariant.InfinityInvariantFor("P1")
            }};
        
        var journey = JourneyCollection.ConstructJourneysFor([("P1", [generator.GenerateSingle(ProcessingLocationStrategy.OnlyProcessingLocations)])]);
        var placeFactory = new PlaceFactory(new ColourTypeFactory(target.Invariants.Select(e=>e.PartType), journey));

        var place = placeFactory.CreatePlace(target);
        var oldText = OldPlaceText(journey, [target]);        
        var newText = NewText([place]);
        newText.Should().Be(oldText);
    }
    
    [Fact]
    public void WhenWritingSingleProcessingLocationsShould_WriteEquivalent()
    {
        var generator =  new LocationGenerator(["P1"]);
        var target = generator.GenerateSingle(ProcessingLocationStrategy.OnlyProcessingLocations);
        var journey = JourneyCollection.ConstructJourneysFor([("P1", [generator.GenerateSingle(ProcessingLocationStrategy.OnlyProcessingLocations)])]);
        var placeFactory = new PlaceFactory(new ColourTypeFactory(target.Invariants.Select(e=>e.PartType), journey));

        var place = placeFactory.CreatePlace(target);
        var oldText = OldPlaceText(journey, [target]);        
        var newText = NewText([place]);
        newText.Should().Be(oldText);
    }
 
    
    
    [Fact (Skip = "Originally we only create 'infinity places' for non-processing locations. We are more detailed now.")]
    public void WhenWritingBothTypesOfLocations_ShouldWriteEquivalent()
    {
        var (_, journey, allLocations) = CreateJourneyAndLocations();
        var placeFactory = CreatePlaceFactory(journey);

        var placeCapPlacePair = allLocations.Select(loc => placeFactory.CreatePlaceAndCapacityPlacePair(loc)).ToArray();
        var capacityPlaces = placeCapPlacePair.Select(e => e.capacityPlace);
        var nonCapacity = placeCapPlacePair.Select(e => e.place);
        var capacityLocations = allLocations.Select(e => e.ToCapacityLocation());
        
        var oldText = OldTranslationText(journey, allLocations, capacityLocations);        
        var newText = NewText([..capacityPlaces, ..nonCapacity]);
        
        newText.Should().Be(oldText);
    }


}