using Common;
using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Colours.Values;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class EmptyAfterAdapterTest : TransitionAttacherTest
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

    private static (Location location, EmptyAfterAdapter adapter) SetupWithConsumption(
        int amountToConsume)
    {
        Location location = CreateLocation(true);
        Location[] emptyAfterWithFrom = [location];
        var partsToConsume = new Dictionary<string, int>();
        partsToConsume.Add(PartType, amountToConsume);
        var adapter = new EmptyAfterAdapter(emptyAfterWithFrom, location, partsToConsume, GetJourneys(location).ToIndexedJourney());
        return (location, adapter);
    }
    
    private static (Location location, EmptyAfterAdapter adapter) SetupFullConsumption()
    {
        Location location = CreateLocation(true);
        Location[] emptyAfterWithFrom = [location];
        var partsToConsume = new Dictionary<string, int>();
        partsToConsume.Add(PartType, location.Capacity);
        var adapter = new EmptyAfterAdapter(emptyAfterWithFrom, location, partsToConsume, GetJourneys(location).ToIndexedJourney());
        return (location, adapter);
    }

    protected HashSet<Location> emptyAfter =
    [
        new("MustBeEmpty", 10, new List<Invariant>
        {
            new(PartType, 0, Infteger.PositiveInfinity)
        }, true),

        new("MustBeEmptyToo", 10, new List<Invariant>(), true)
    ];
    
    public override ITransitionAttachable CreateFromLocation(Location location)
    {
        var partsToConsume = new Dictionary<string, int>();
        partsToConsume.Add(PartType, 3);
        return new EmptyAfterAdapter(emptyAfter, location, partsToConsume, GetJourneys(location).ToIndexedJourney());
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