using JsonFixtures;
using TACPN.Colours.Values;
using TestDataGenerator;
using Tmpms;
using Tmpms.Journey;
using Tmpms.Move;
using TmpmsPetriNetAdapter.Colours;
using TmpmsPetriNetAdapter.TransitionFactory;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionFactory;

public abstract class MoveActionDependentTest : IClassFixture<MoveActionFixture>
{
    public Func<IDictionary<string, int>, ColourVariable[]> CreateVariables { get; set; }
    public readonly Location BufferLocation;
    public readonly Location ProcessingLocation;
    protected readonly IEnumerable<Location> _otherLocations;

    public static IEnumerable<string> Parts = Enumerable.Range(1, 5).Select(e => "P" + e);
    public string P1 = Parts.First();
    public string P2 = Parts.Skip(1).First();
    public string P3 = Parts.Skip(2).First();
    public string P4 = Parts.Skip(3).First();
    public string P5 = Parts.Skip(4).First();
    

    
    public Dictionary<string, int> Move(params (int amount, string partType)[] parts)
    {
        return parts.Select(e => KeyValuePair.Create(e.partType, e.amount)).ToDictionary();
    }

    protected MoveAction CreateMoveAction(Location from, Location to, Dictionary<string, int> partsToMove)
    {
        return new MoveAction()
        {
            Name = "Test action",
            From = from,
            To = to,
            EmptyAfter = new HashSet<Location>(),
            EmptyBefore = new HashSet<Location>(),
            PartsToMove = partsToMove
        };
    }

    protected ITransitionFactory CreateTransitionFactoryForJourneys(JourneyCollection collection)
    {
        var ctf = CreateColourTypeFactory(collection);
        return new MoveActionTransitionFactory(ctf, new ColourVariableFactory(ctf));
    }

    protected static ColourTypeFactory CreateColourTypeFactory(JourneyCollection collection)
    {
        return new ColourTypeFactory(Parts, collection);
    }


    public MoveActionDependentTest(MoveActionFixture fixture)
    {
        var LocationGenerator = fixture.CreateLocationGenerator(Parts);
        this.ProcessingLocation = LocationGenerator.GenerateSingle(ProcessingLocationStrategy.OnlyProcessingLocations);
        this.BufferLocation = LocationGenerator.GenerateSingle(ProcessingLocationStrategy.OnlyRegularLocations);
        this.CreateVariables = MoveActionFixture.VariablesForParts;
        this._otherLocations = LocationGenerator.Generate(10);
    }
}