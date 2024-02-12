using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.Colours;

namespace TaapalParser.UnitTests;

public abstract class WriterTest : NoWhitespaceWriterTest, IClassFixture<MoveActionFixture>
{
    public readonly JourneyCollection Journeys;
    public ColourTypeFactory ColourFactory { get; set; }
    public ProductColourType TokensColourType { get; set; }
    public ColourType JourneyColour { get; set; }
    public SingletonColourType Dot { get; set; }
    public ColourType PartsColourType { get; set; }
    public string[] Parts { get; set; }
    public readonly ColourVariableFactory VariableFactory;  

    public WriterTest(MoveActionFixture fixture)
    {
        Parts = ["P1", "P2", "P3"];
        PartsColourType = new ColourType(Colour.PartsColour.Value, Parts);
        Dot = ColourType.DefaultColorType;
        
        Journeys = JourneyCollection.ConstructJourneysFor(CreateJourneyComponents(fixture));
        ColourFactory = new ColourTypeFactory(Parts, Journeys);
        VariableFactory = new ColourVariableFactory(ColourFactory);
        
        
        JourneyColour = ColourFactory.Journey;
        TokensColourType = ColourFactory.Tokens;
    }

    private (string partType, IEnumerable<Location> locations)[] CreateJourneyComponents(MoveActionFixture fixture)
    {
        LocationGenerator LocationGenerator = fixture.CreateLocationGenerator(Parts);
        return [
            ("P1", LocationGenerator.Generate(8)),
            ("P2", LocationGenerator.Generate(3)),
            ("P3", LocationGenerator.Generate(4))
        ];
    }




}