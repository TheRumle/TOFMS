using Common;
using FluentAssertions;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionFactory;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionFactory;

public class MoveActionTransitionFactoryTest : IClassFixture<MoveActionFixture>
{
    public MoveActionTransitionFactoryTest(MoveActionFixture fixture)
    {
        this.ProcessingLocation = fixture.ProcessingLocation;
        this.BufferLocation = fixture.BufferLocation;
        this.Variables = MoveActionFixture.VariablesForParts;
        this._otherLocations = fixture.LocationGenerator.GenerateLocations(10);
    }

    public readonly ColourVariable[] Variables;

    public readonly Location BufferLocation;
    public readonly Location ProcessingLocation;
    private readonly IEnumerable<Location> _otherLocations;

    public HashSet<KeyValuePair<string, int>> Move(params (int amount, string partType)[] parts)
    {
        return parts.Select(e => KeyValuePair.Create(e.partType, e.amount)).ToHashSet();
    }

    protected MoveAction CreateMoveAction(Location from, Location to, HashSet<KeyValuePair<string, int>> partsToMove)
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
    

    [Fact]
    public void When_IndexIsEqualToNextStep_ShouldHaveGuard()
    {
        var moveAction = CreateMoveAction(ProcessingLocation, BufferLocation,Move((3, "P1")));
        var to = moveAction.To;

        JourneyCollection journeys = JourneyCollection.ConstructJourneysFor(
        [
                ("P1", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(to,3)])), 
                ("P2", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(to,2)])),
                ("P3", ListExtensions.Shuffle([.._otherLocations, to]))
        ]);
        
        var transitionFactory = CreateTransitionFactoryForJourneys(journeys);
        var transition = transitionFactory.CreateTransition(moveAction);
        //Assert that transition has guards for all 3 times 


        var variableOccurances = transition.Guard.Statements.SelectMany(e => e.Conditions.Select(comparison => comparison.Lhs));
        variableOccurances.Where(e => e.IsVariableFor("P1")).Should().HaveCount(3);
        variableOccurances.Where(e => e.IsVariableFor("P2")).Should().HaveCount(2);
        variableOccurances.Where(e => e.IsVariableFor("P3")).Should().HaveCount(1);
    }

    private ITransitionFactory CreateTransitionFactoryForJourneys(JourneyCollection collection)
    {
        return new MoveActionTransitionFactory(collection.ToIndexedJourney(), Variables);
    }
}

public class MoveActionFixture
{
    public readonly static IEnumerable<string> Parts = ["P1", "P2", "P3"];
    public Location ProcessingLocation { get; set; } = new Location("From", 3, new Invariant[]
    {
        new Invariant("P1", 0, 3),
        new Invariant("P2", 0, 5),
        new Invariant("P3", 3, 5)
    }, true);

    public Location BufferLocation { get; set; } = new Location("From", 3, Invariant.InfinityInvariantsFor(Parts), false);

    public static  readonly ColourVariable[] VariablesForParts =
        Parts.Select(e => ColourVariable.CreateFromPartName(e, ColourType.PartsColourType)).ToArray();

    public readonly LocationGenerator LocationGenerator;


    public MoveActionFixture()
    {
        LocationGenerator = new LocationGenerator(Parts);
    }
}