using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Colours.Values;
using TACPN.Transitions;
using Tmpms.Common;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class FromTransitionAttacherTest : TransitionAttacherTest
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
            var ingoing = transition.InGoing.First(e => e.From.Name.Equals(location.Name));
            ingoing.ArcExpression.ExpressionText.Should()
                .Be($"4'({PartType}, {variableExpressionValue})");

            var outgoing = transition.OutGoing.First(e => e.To.Name.Contains(location.Name));
            outgoing.Expression.ExpressionText.Should().Be($"4'{Colour.DefaultTokenColour}");
        }
    }
    
    
    
    public override ITransitionAttachable CreateFromLocation(Location location)
    {
        return new FromLocationAdaption(location, new[]
        {
            new KeyValuePair<string, int>(PartType, 4)
        }, getJourneys(location).ToIndexedJourney());
    }
}