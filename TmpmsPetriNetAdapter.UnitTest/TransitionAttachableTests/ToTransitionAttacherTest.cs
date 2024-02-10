using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures;
using TACPN.Colours.Type;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class ToTransitionAttacherTest : TransitionAttacherTest
{
    [Fact]
    public void ShouldHaveCorrectArcExpressions_WhenFrom_IsProcessing()
    {
        var from = CreateLocation(true);
        var to   = CreateLocation(false);
        var journey = SingletonJourney(from);

        MoveAction action = CreateMoveAction(from,to);
        ToTransitionAttacher attacher = new ToTransitionAttacher(action, CreateColourTypeFactory(journey), journey.ToIndexedJourney());
        attacher.AttachToTransition(Transition);


        using (new AssertionScope())
        {
            var outGoingArc = Transition.OutGoing.First(e => e.To.Name == from.Name);
            outGoingArc.Expression.ExpressionText.Should().Be($"4'({PartType}, )");
            
            var inGoingFromCapacityPlace = Transition.InGoing.First(e => e.From.Name.Contains(to.Name));
            inGoingFromCapacityPlace.Expression.ExpressionText.Should().Be($"4'{ColourType.DefaultColorType.ColourValue}");
        }
    }

    public ToTransitionAttacherTest(MoveActionFixture moveActionFixture) : base(moveActionFixture)
    {   
    }
}