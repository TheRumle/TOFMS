using System.Text;
using Common;
using FluentAssertions;
using JsonFixtures;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;
using TmpmsPetriNetAdapter.TransitionFactory;
using TransitionWriter = TapaalParser.TapaalGui.Writers.TransitionWriter;

namespace TaapalParser.UnitTests.TapaalGui.DeclarationWriters;

public class TransitionWriterTest : NoWhitespaceWriterTest, IClassFixture<MoveActionFactoryFixture>
{
    private string[] PartTypes = ["P1"];
    private TransitionWriter _writer;
    private readonly MoveActionFactoryFixture moveActionFactory;

    public LocationGenerator BufferLocationGenerator { get; set; }
    public LocationGenerator ProcessingLocationGenerator { get; set; }
    
    MoveActionTransitionFactory CreateTransitionFactory(JourneyCollection journeyCollection)
    {
        return new MoveActionTransitionFactory(new ColourTypeFactory(PartTypes, journeyCollection));
    }

    public TransitionWriterTest(MoveActionFactoryFixture fixture)
    {
        this.ProcessingLocationGenerator = new LocationGenerator(PartTypes, ProcessingLocationStrategy.OnlyProcessingLocations);
        this.BufferLocationGenerator = new LocationGenerator(PartTypes, ProcessingLocationStrategy.OnlyRegularLocations);
        this.moveActionFactory = fixture;
    }
        
    [Fact]
    public void When_To_IsInJourney()
    {
        var from = BufferLocationGenerator.GenerateSingle();
        var to = ProcessingLocationGenerator.GenerateSingle();
        var action = this.moveActionFactory.CreateMoveAction(from, to, [], [],
            [(PartTypes.First(), from.Capacity)]);
        
        var journey = CreateJourney(to);


        Xml.TransitionWriter oldWriter = new Xml.TransitionWriter(new StringBuilder(),action);
        oldWriter.WriteTransition(journey.ToIndexedJourney());
        var oldText = RemoveWhiteSpace(oldWriter.StringBuilder.ToString());
        
        var transition = CreateTransitionFactory(journey).CreateTransition(action);
        TransitionWriter newWriter = new TransitionWriter(transition);
        newWriter.AppendAllText();

        var newText = RemoveWhiteSpace(newWriter.ToString());


        newText.Should().Be(oldText);


    }

    private JourneyCollection CreateJourney(Location mustInclude)
    {
        IEnumerable<Location> sequence = ListExtensions.Shuffle([mustInclude, mustInclude]);
        var journey = JourneyCollection.ConstructJourneysFor([(PartTypes.First(),sequence)]);
        return journey;
    }
}