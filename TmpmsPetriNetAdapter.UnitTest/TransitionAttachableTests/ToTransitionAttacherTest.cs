using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class ToTransitionAttacherTest : TransitionAttacherTest
{



    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldHaveCorrectArcExpressions(bool isProcessingLocation)
    {
        var variableExpressionValue = GetExpectedVariableExpressionValue(isProcessingLocation);
        (Transition transition, Location location) = CreateAndAttach(isProcessingLocation);

        using (new AssertionScope())
        {
            var outGoingArc = transition.OutGoing.First(e => e.To.Name == location.Name);
            outGoingArc.Expression.ExpressionText.Should().Be($"4'({PartType}, {variableExpressionValue})");
            
            var inGoingFromCapacityPlace = transition.InGoing.First(e => e.From.Name.Contains(location.Name));
            inGoingFromCapacityPlace.Expression.ExpressionText.Should().Be($"4'{ColourType.DefaultColorType.ColourValue}");
        }
    }
    
    public override ITransitionAttachable CreateFromLocation(Location location)
    {
        return new ToTransitionAttacher(location, new[]
        {
            new KeyValuePair<string, int>(PartType, 4)
        }, GetJourneys(location).ToIndexedJourney());
    }
}