using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Places;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.UnitTest;

public class PlaceFactoryTest {
    private readonly LocationGenerator Generator;
    private readonly Location LocationToTranslate;
    public readonly string[] Parts;
    public JourneyCollection JourneyCollection { get; set; }

    public PlaceFactoryTest()
    {

        this.Parts = Enumerable.Range(1,10).Select(e=>"P"+e).ToArray();
        this.Generator = new LocationGenerator(Parts, markingStrategy: MarkingStrategy.None, processingLocationStrategy: ProcessingLocationStrategy.Both);
        this.LocationToTranslate = Generator.GenerateSingle();
        this.JourneyCollection = CreateJourney(LocationToTranslate);
    }

    private JourneyCollection CreateJourney(Location target)
    {
        return JourneyCollection.ConstructJourneysFor(Parts.Select(e => (e, ListExtensions.Shuffle([target, ..Generator.Generate(10)]))).ToArray());
    }


    private PlaceFactory CreateFactory(JourneyCollection journeyCollection)
    {
        return new PlaceFactory(new ColourTypeFactory(Parts, journeyCollection), journeyCollection.ToIndexedJourney());
    }

    [Theory]
    [InlineData(10, 10)]
    [InlineData(8, 3)]
    [InlineData(2, 1)]
    [InlineData(1, 1)]
    [InlineData(0, 0)]
    public void InitializesCapacityLocationsWith_AmountOfTokens_AsTheCapacityMinusConfiguration(int capacity, int numParts)
    {
        List<Part> parts = [..Enumerable.Repeat(new Part(Parts.First(), new Random().Next()), numParts)];
        var configuration = new LocationConfiguration
        {
            { Parts.First(), parts }
        };

        var target = Generator.GenerateSingle() with { Configuration = configuration, Capacity = capacity};
        var journeyCollection = CreateJourney(target);
        var factory = CreateFactory(journeyCollection);

        
        Place place = factory.CreateInitializedCapacityPlaceFor(target);
        place.Marking.Size.Should().Be(target.Capacity - numParts);
    }


    [Fact]
    public void InitializesCapacityLocationsWithAgeZero()
    {
        var factory = CreateFactory(JourneyCollection);

        Place place = factory.CreateInitializedCapacityPlaceFor(LocationToTranslate);

        place.Marking.Tokens.Should().AllSatisfy(e => e.Age.Should().Be(0));
    } 
}