using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Colours.Values;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class FromTransitionAttacherTest
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
            ingoing.Expression.ExpressionText.Should()
                .Be($"4'({PartType}, {variableExpressionValue})");

            var outgoing = transition.OutGoing.First(e => e.To.Name.Contains(location.Name));
            outgoing.Expression.ExpressionText.Should().Be($"4'{Colour.DefaultTokenColour}");
        }
    }
    
    
    
    public override ITransitionAttachable CreateFromLocation(Location from, Location to)
    {
        var journeys = GetJourneyWithNextTarget(from).ToIndexedJourney();
        MoveAction move = new MoveAction()
        {
            Name = "Test",
            EmptyAfter = { },
            PartsToMove = [new KeyValuePair<string, int>(PartType, 4)],
            EmptyBefore = { },
            From = from,
            To = to
        };
        
        return new FromLocationAdaption(move, this.ColourTypeFactory, journeys);
    }
}