using Common;
using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Colours.Values;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class EmptyAfterAdapterTest
{
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ShouldCreateArcFromCapPlace_AndRestoreCapacity(int amountToConsume)
    {
        (Location location, EmptyAfterAdapter adapter) = SetupWithConsumption(amountToConsume);
        (Transition transition, _) = CreateAndAttach(adapter, location);
        
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().BeEmpty();
            
            transition.InGoing.First(e => e.From.IsCapacityLocation).Expression.ExpressionText
                .Should().Be($"{location.Capacity - amountToConsume}'{Colour.DefaultTokenColour}");
            
            transition.OutGoing.First(e => e.To.IsCapacityLocation).Expression.ExpressionText
                .Should().Be($"{location.Capacity}'{Colour.DefaultTokenColour}");
        }
    }
    
    [Fact]
    public void WhenConsumeIsEqualToCapacity_ShouldNotHaveArc_FromCapplace_ToTransition()
    {
        
        (Location location, EmptyAfterAdapter adapter) = SetupFullConsumption();
        (Transition transition, _) = CreateAndAttach(adapter, location);
        
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().BeEmpty();
            transition.InGoing.Where(e => e.From.IsCapacityLocation).Should().BeEmpty();
            transition.OutGoing.First(e => e.To.IsCapacityLocation).Expression.ExpressionText
                .Should().Be($"{location.Capacity}'{Colour.DefaultTokenColour}");
        }
    }

    private (Location location, EmptyAfterAdapter adapter) SetupWithConsumption(
        int amountToConsume)
    {
        Location from = CreateLocation(true);
        Location to = CreateLocation(false);
        Location[] emptyAfterWithFrom = [from];
        var partsToConsume = new Dictionary<string, int>();
        partsToConsume.Add(PartType, amountToConsume);
        var adapter = CreateAdapter(emptyAfterWithFrom, from,to, partsToConsume);
        return (from, adapter);
    }

    private EmptyAfterAdapter CreateAdapter(Location[] emptyAfterWithFrom, Location from, Location to, Dictionary<string, int> partsToConsume)
    {
        var move = new MoveAction()
        {
            Name = "Test",
            EmptyAfter = emptyAfterWithFrom.ToHashSet(),
            From = from,
            To = to,
            EmptyBefore = new HashSet<Location>(),
            PartsToMove = partsToConsume.ToHashSet()
        };
        return new EmptyAfterAdapter(move, this.ColourTypeFactory, GetJourneyWithNextTarget(from).ToIndexedJourney());
    }

    private (Location location, EmptyAfterAdapter adapter) SetupFullConsumption()
    {
        Location from = CreateLocation(true);
        Location to = CreateLocation(false);
        Location[] emptyAfterWithFrom = [from];
        var partsToConsume = new Dictionary<string, int>();
        partsToConsume.Add(PartType, from.Capacity);
        var adapter = CreateAdapter(emptyAfterWithFrom, from,to, partsToConsume);
        return (from, adapter);
    }
    protected HashSet<Location> emptyAfter =
    [
        new("MustBeEmpty", 10, new List<Invariant>
        {
            new(PartType, 0, Infteger.PositiveInfinity)
        }, true),

        new("MustBeEmptyToo", 10, new List<Invariant>(), true)
    ];
    
    public override ITransitionAttachable CreateFromLocation(Location from, Location to)
    {
        MoveAction move = new MoveAction()
        {
            Name = "Test",
            EmptyAfter = emptyAfter,
            PartsToMove = [new KeyValuePair<string, int>(PartType, 4)],
            EmptyBefore = { },
            From = from,
            To = to
        };
        
        return new EmptyAfterAdapter(move, ColourTypeFactory, GetJourneyWithNextTarget(from).ToIndexedJourney());
    }
    
    [Fact]
    public void ShouldHaveCorrectWeightFromCorrectArcs()
    {
        (Transition transition, _) = CreateAndAttach(true);
        
        var emptyBeforeNames = emptyAfter.Select(l => l.Name);
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().AllSatisfy(e => e.Weight.Should().Be(1));
            transition.InhibitorArcs.Should().AllSatisfy(e => emptyBeforeNames.Should().Contain(e.From.Name));
        }
    }
    
    

}