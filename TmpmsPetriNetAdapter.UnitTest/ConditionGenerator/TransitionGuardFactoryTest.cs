using Common;
using FluentAssertions;
using JsonFixtures;
using TACPN.Transitions.Guard;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.ConditionGenerator;
using TmpmsPetriNetAdapter.TransitionFactory;
using TmpmsPetriNetAdapter.UnitTest.TransitionFactory;

namespace TmpmsPetriNetAdapter.UnitTest.ConditionGenerator;

public class TransitionGuardFactoryTest : MoveActionDependentTest
{
    
    
    
    [Theory]
    [InlineData(1, 1)]
    public void WhenMovingTwoItems_WithJourneyLengths_n_i_ShouldGive_N_OrStatements(int n, int i)
    {
        var moveAction = CreateMoveAction(ProcessingLocation, BufferLocation,Move((3, "P1")));
        JourneyCollection collection = new JourneyCollection();
        var generator = new TransitionGuardFactory(collection);
        TransitionGuard a = generator.MoveActionGuard(moveAction);
        
        a.Count().Should()
        
    }
    
    private ITransitionFactory CreateTransitionFactoryForJourneys(JourneyCollection collection)
    {
        return new MoveActionTransitionFactory(collection.ToIndexedJourney(), CreateVariables(collection.JourneyLengths()));
    }


    public TransitionGuardFactoryTest(MoveActionFixture fixture) : base(fixture)
    {
    }
}