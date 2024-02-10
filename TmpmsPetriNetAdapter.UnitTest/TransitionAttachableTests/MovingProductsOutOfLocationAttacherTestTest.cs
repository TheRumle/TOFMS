using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class MovingProductsOutOfLocationAttacherTestTest : TransitionAttacherTest
{
    
    [Fact]
    public void ShouldHaveCorrectArcExpressions_WhenFrom_IsNotProcessing()
    {
        var from = CreateLocation(false);
        var to   = CreateLocation(false);
        var transition = CreateAndAttachToTransition(from, to);

        
        using (new AssertionScope())
        {
            //Restore capacity by producing tokens to capacity place
            var outgoing = transition.OutGoing.First(e => e.To.Name.Contains(from.Name));
            outgoing.Expression.ExpressionText.Should().Be($"4'{Colour.DefaultTokenColour}");
            
            //Consume products by create ingoing arc removing the part
            var ingoing = transition.InGoing.First(e => e.From.Name.Equals(from.Name));
            ingoing.Expression.ExpressionText.Should()
                .Be($"4'({PartType}, {ColourVariableFactory.VariableNameFor(PartType)})");
        }
    }
    
    [Fact]
    public void ShouldHaveCorrectArcExpressions_WhenFrom_IsProcessing()
    {
        var from = CreateLocation(true);
        var to   = CreateLocation(true);
        var transition = CreateAndAttachToTransition(from, to);

        using (new AssertionScope())
        {
            //Restore capacity by producing tokens to capacity place
            var outgoing = transition.OutGoing.First(e => e.To.Name.Contains(from.Name));
            outgoing.Expression.ExpressionText.Should().Be($"4'{Colour.DefaultTokenColour}");
            
            //Consume products by create ingoing arc removing the part
            var ingoing = transition.InGoing.First(e => e.From.Name.Equals(from.Name));
            ingoing.Expression.ExpressionText.Should()
                .Be($"4'({PartType}, {ColourVariableFactory.VariableNameFor(PartType)}--)");
        }
    }
    
    protected Transition CreateAndAttachToTransition(Location from, Location to)
    {
        var journey = SingletonJourney(from);
        var transition = CreateTransition(journey);
        MoveAction action = CreateMoveAction(from,to);
        MovingProductsOutOfLocationAttacherTest attacher = new MovingProductsOutOfLocationAttacherTest(action, CreateColourTypeFactory(journey), journey.ToIndexedJourney());
        attacher.AttachToTransition(transition);
        return transition;
    }


    public MovingProductsOutOfLocationAttacherTestTest(MoveActionFixture moveActionFixture) : base(moveActionFixture)
    {
    }
}