using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class ToTransitionAttacherTest : TransitionAttacherTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldHaveCorrectArcExpressions(bool isFromProcessing)
    {
        var variableExpressionValue = GetExpectedVariableExpressionValue(isFromProcessing);
        (Transition transition, Location location) = CreateAndAttach(isFromProcessing);

        using (new AssertionScope())
        {
            var outGoingArc = transition.OutGoing.First(e => e.To.Name == location.Name);
            outGoingArc.Expression.ExpressionText.Should().Be($"4'({PartType}, {variableExpressionValue})");
            
            var inGoingFromCapacityPlace = transition.InGoing.First(e => e.From.Name.Contains(location.Name));
            inGoingFromCapacityPlace.Expression.ExpressionText.Should().Be($"4'{ColourType.DefaultColorType.ColourValue}");
        }
    }
    
    public override ITransitionAttachable CreateFromLocation(Location from, Location to)
    {
        MoveAction move = new MoveAction()
        {
            Name = "Test",
            EmptyAfter = { },
            PartsToMove = [new KeyValuePair<string, int>(PartType, 4)],
            EmptyBefore = { },
            From = from,
            To = to
        };
        
        
        
        return new ToTransitionAttacher(move, this.ColourTypeFactory, GetJourneys(from).ToIndexedJourney());
    }
}