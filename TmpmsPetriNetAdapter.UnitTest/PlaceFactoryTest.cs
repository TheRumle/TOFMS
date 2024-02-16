using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Places;
using TestDataGenerator;
using Tmpms;
using Tmpms.Journey;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.UnitTest;

public class PlaceFactoryTest {
    private readonly LocationGenerator Generator;
    public readonly string[] Parts = Enumerable.Range(1, 10).Select(e => "P" + e).ToArray();

    public PlaceFactoryTest()
    {
        this.Generator = new LocationGenerator(Parts, markingStrategy: MarkingStrategy.None, processingLocationStrategy: ProcessingLocationStrategy.Both);
    }

    private JourneyCollection CreateJourney(Location target)
    {
        return JourneyCollection.ConstructJourneysFor(Parts.Select(e => (e, ListExtensions.Shuffle([target, ..Generator.Generate(10, ProcessingLocationStrategy.OnlyProcessingLocations)]))).ToArray());
    }

    private PlaceFactory CreateFactory(IEnumerable<string> parts, JourneyCollection journeyCollection)
    {
        return new PlaceFactory(new ColourTypeFactory(parts, journeyCollection));
    }

    private PlaceFactory CreateFactory(JourneyCollection journeyCollection)
    {
        return new PlaceFactory(new ColourTypeFactory(Parts, journeyCollection));
    }

    [Theory]
    [InlineData(10, 10)]
    [InlineData(8, 3)]
    [InlineData(2, 1)]
    [InlineData(1, 1)]
    [InlineData(0, 0)]
    public void InitializesCapacityLocationsWith_AmountOfTokens_AsTheCapacityMinusConfiguration(int capacity, int numParts)
    {
        List<Part> parts = [..Enumerable.Repeat(new Part(Parts.First(), new Random().Next(), []), numParts)];
        var configuration = new LocationConfiguration(Parts);
        configuration.Add(parts);

        var target = Generator.GenerateSingle() with { Configuration = configuration, Capacity = capacity};
        var journeyCollection = CreateJourney(target);
        var factory = CreateFactory(journeyCollection);

        
        Place place = factory.CreateInitializedCapacityPlaceFor(target);
        place.Marking.Size.Should().Be(target.Capacity - numParts);
    }


    
    [Theory]
    [InlineData(10,10)]
    [InlineData(1,1)]
    [InlineData(2,2)]
    [InlineData(3,1)]
    public void When_HavingNInvariants_AndIJourneyLenght_Creates_NTimesI_Invariants(int journeyLength, int numberParts)
    {
        var parts = Enumerable.Range(1,numberParts).Select(e=>"P"+e).ToArray();
        var target = new LocationGenerator(parts).GenerateSingle(ProcessingLocationStrategy.OnlyProcessingLocations);
        target.Invariants.Should().HaveCount(parts.Length);
        
        var journeys = parts.Select(p => (p, Enumerable.Repeat(target, journeyLength)));

        var factory = CreateFactory(parts, JourneyCollection.ConstructJourneysFor(journeys.ToArray()));
        Place place = factory.CreatePlace(target);

        place.ColourInvariants.Should().HaveCount(journeyLength*numberParts);
    }


    [Fact]
    public void InitializesCapacityLocationsWithAgeZero()
    {
        var target = Generator.GenerateSingle(ProcessingLocationStrategy.OnlyProcessingLocations);
        var factory = CreateFactory(CreateJourney(target));

        Place place = factory.CreateInitializedCapacityPlaceFor(target);

        place.Marking.Tokens.Should().AllSatisfy(e => e.Age.Should().Be(0));
    } 
}