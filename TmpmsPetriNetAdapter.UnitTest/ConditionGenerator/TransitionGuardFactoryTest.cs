using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Transitions.Guard;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.ConditionGenerator;
using TmpmsPetriNetAdapter.UnitTest.TransitionFactory;

namespace TmpmsPetriNetAdapter.UnitTest.ConditionGenerator;

public class TransitionGuardFactoryTest : MoveActionDependentTest
{
    [Fact]
    public void WhenMoving1PartType_ShouldHave1Condition()
    {
        var moveAction = CreateMoveAction(ProcessingLocation, BufferLocation,Move((3, P1)));
        JourneyCollection journeys = JourneyCollection.ConstructJourneysFor([
            ("P1", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(moveAction.To, 5)]))
        ]);
        
        var generator = new TransitionGuardFactory(journeys, PartsColourType);
        TransitionGuard a = generator.MoveActionGuard(moveAction);

        a.Conditions.Should().HaveCount(1);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public void WhenMoving1PartType_ShouldHave1Condition_WithNComparisons(int journeyLength)
    {
        var moveAction = CreateMoveAction(ProcessingLocation, BufferLocation, Move((3, "P1")));
        JourneyCollection journeys = JourneyCollection.ConstructJourneysFor([
            ("P1", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(moveAction.To, journeyLength)]))
        ]);
        
        var generator = new TransitionGuardFactory(journeys, PartsColourType);
        TransitionGuard a = generator.MoveActionGuard(moveAction);

        a.Conditions.First().Comparisons.Should().HaveCount(journeyLength);
    }
    

    
    [Fact]
    public void WhenMoving2PartType_ShouldHave2Conditions()
    {
        var moveAction = CreateMoveAction(ProcessingLocation, BufferLocation,Move((3, "P1"), (2,"P2")));
        JourneyCollection journeys = JourneyCollection.ConstructJourneysFor([
            ("P1", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(moveAction.To, 5)])),
            ("P2", ListExtensions.Shuffle([.._otherLocations, ..Enumerable.Repeat(moveAction.To, 5)]))
        ]);
        
        var generator = new TransitionGuardFactory(journeys, PartsColourType);
        TransitionGuard a = generator.MoveActionGuard(moveAction);

        a.Conditions.Should().HaveCount(2);
    }


    public TransitionGuardFactoryTest(MoveActionFixture fixture) : base(fixture)
    {
    }
    
}