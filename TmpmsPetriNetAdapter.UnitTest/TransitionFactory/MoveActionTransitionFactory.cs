using System.Runtime.InteropServices;
using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Colours.Values;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionFactory;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionFactory;

public class MoveActionTransitionFactoryTest : IClassFixture<MoveActionFixture>
{
    public MoveActionTransitionFactoryTest(MoveActionFixture fixture)
    {
        this.ProcessingLocation = fixture.ProcessingLocation;
        this.BufferLocation = fixture.BufferLocation;
        this.CreateVariables = MoveActionFixture.VariablesForParts;
        this._otherLocations = fixture.LocationGenerator.GenerateLocations(10);
    }

    public Func<IDictionary<string, int>, ColourVariable[]> CreateVariables { get; set; }

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
    

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void When_IndexIsEqualToNextStep_ShouldHaveGuard(int numJourneyOccurances)
    {
        var moveAction = CreateMoveAction(ProcessingLocation, BufferLocation,Move((3, "P1")));

        JourneyCollection journeys = JourneyCollection.ConstructJourneysFor([
                ("P1", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(moveAction.To, numJourneyOccurances)]))
        ]);
        
        var transitionFactory = CreateTransitionFactoryForJourneys(journeys);
        var transition = transitionFactory.CreateTransition(moveAction);


        var variableOccurances = transition.Guard.OrStatements.SelectMany(e => e.Comparisons.Select(comparison => comparison.Lhs));
        variableOccurances.Where(e => e.IsVariableFor("P1")).Should().HaveCount(numJourneyOccurances);
    }

    
    [Theory]
    [InlineData(1,1)]
    public void When_MovingMultiplePartTypes_ShouldHaveAndGuard(int jourP1, int jourP2)
    {
        var moveAction = CreateMoveAction(ProcessingLocation, BufferLocation, Move((3, "P1"), (2,"P2")));
        
        JourneyCollection journeys = JourneyCollection.ConstructJourneysFor([
            ("P1", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(moveAction.To, jourP1)])), 
            ("P2", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(moveAction.To, jourP2)])) 
        ]);
        
        var transitionFactory = CreateTransitionFactoryForJourneys(journeys);
        ITransitionGuard transitionGuard = transitionFactory.CreateTransition(moveAction).Guard;
        

    }
    
    private ITransitionFactory CreateTransitionFactoryForJourneys(JourneyCollection collection)
    {
        return new MoveActionTransitionFactory(collection.ToIndexedJourney(), CreateVariables(collection.JourneyLengths()));
    }
}