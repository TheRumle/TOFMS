using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Colours.Type;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionFactory;

public class MoveActionTransitionFactoryTest : MoveActionDependentTest
{
    public MoveActionTransitionFactoryTest(MoveActionFixture fixture) : base(fixture)
    {
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


        var variableOccurances = transition.Guard.Conditions.SelectMany(e => e.Comparisons.Select(comparison => comparison.Lhs));
        variableOccurances.Where(e => e.IsVariableFor("P1")).Should().HaveCount(numJourneyOccurances);
    }
}