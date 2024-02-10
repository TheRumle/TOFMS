using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class MovingProductsIntoLocationAdapterTest : TransitionAttacherTest
{
    protected Transition CreateAndAttachToTransition(Location from, Location to)
    {
        var journey = SingletonJourney(from);
        var transition = CreateTransition(journey);
        MoveAction action = CreateMoveAction(from,to);
        MovingProductsIntoLocationAdapter attacher = new MovingProductsIntoLocationAdapter(action, CreateColourTypeFactory(journey), journey.ToIndexedJourney());
        attacher.AttachToTransition(transition);
        return transition;
    }

    public MovingProductsIntoLocationAdapterTest(MoveActionFixture moveActionFixture) : base(moveActionFixture)
    {   
    }
    
    [Fact]
    public void ShouldHaveCorrectArcExpressions_WhenTo_IsNotProcessing()
    {
        var from = CreateLocation(true);
        var to   = CreateLocation(false);
        var transition = CreateAndAttachToTransition(from, to);

        using (new AssertionScope())
        {
            //There must be arc out of the location we move items into
            var outGoingArc = transition.OutGoing.First(e => e.To.Name == to.Name);
            outGoingArc.Expression.ExpressionText.Should().Be($"4'({PartType}, {ColourVariableFactory.VariableNameFor(PartType)})");
            
            //There must be an arc consuming capacity
            var inGoingFromCapacityPlace = transition.InGoing.First(e => e.From.Name.Contains(to.Name));
            inGoingFromCapacityPlace.Expression.ExpressionText.Should().Be($"4'{ColourType.DefaultColorType.ColourValue}");
        }
    }
    
    [Fact]
    public void ShouldHaveCorrectArcExpressions_WhenTo_IsProcessing()
    {
        var from = CreateLocation(true);
        var to   = CreateLocation(true);
        var transition = CreateAndAttachToTransition(from, to);

        using (new AssertionScope())
        {
            var outGoingArc = transition.OutGoing.First(e => e.To.Name == to.Name);
            outGoingArc.Expression.ExpressionText.Should().Be($"4'({PartType}, {ColourVariableFactory.VariableNameFor(PartType)}--)");
            
            var inGoingFromCapacityPlace = transition.InGoing.First(e => e.From.Name.Contains(to.Name));
            inGoingFromCapacityPlace.Expression.ExpressionText.Should().Be($"4'{ColourType.DefaultColorType.ColourValue}");
        }
    }

    
    

}