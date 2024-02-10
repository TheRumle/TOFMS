using Common;
using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class EmptyBeforeCapacitorInhibitorAdaptionTest : TransitionAttacherTest
{
    protected HashSet<Location> EmptyBef =
    [
        new("MustBeEmpty", 10, new List<Invariant>
        {
            new(PartType, 0, Infteger.PositiveInfinity)
        }, true),

        new("MustBeEmptyToo", 10, new List<Invariant>(), true)
    ];
    
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldHaveCorrectWeightFromCorrectArcs(bool isProcessingLocation)
    {
        (Transition transition, _) = CreateAndAttach(isProcessingLocation);
        
        var emptyBeforeNames = EmptyBef.Select(l => l.Name);
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().AllSatisfy(e => e.Weight.Should().Be(1));
            transition.InhibitorArcs.Should().AllSatisfy(e => emptyBeforeNames.Should().Contain(e.From.Name));
        }
    }
  
    public override ITransitionAttachable CreateFromLocation(Location from, Location to)
    {
        var journeys = GetJourneys(from);
        MoveAction move = new MoveAction()
        {
            Name = "Test",
            EmptyAfter = { },
            PartsToMove = [new KeyValuePair<string, int>(PartType, 4)],
            EmptyBefore = EmptyBef,
            From = from,
            To = to
        };

        return new EmptyBeforeCapacitorInhibitorAdaption(move, this.ColourTypeFactory, journeys.ToIndexedJourney());
    }
}