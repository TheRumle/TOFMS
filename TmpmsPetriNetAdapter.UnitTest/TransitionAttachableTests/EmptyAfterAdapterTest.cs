using Common;
using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class EmptyAfterAdapterTest : TransitionAttacherTest
{
    public EmptyAfterAdapterTest(MoveActionFixture moveActionFixture) : base(moveActionFixture)
    {
    }

    protected Transition CreateAndAttachToTransition(Location from, Location to, int amountToConsume, IEnumerable<Location> otherEmptyAfter)
    {
        var journey = SingletonJourney(to);
        var transition = CreateTransition(journey);
        MoveAction action = CreateMoveAction(from, to, new HashSet<Location>(), [from,..otherEmptyAfter], amountToConsume);
        EmptyAfterAdapter attacher = new EmptyAfterAdapter(action, CreateColourTypeFactory(journey), journey.ToIndexedJourney());
        attacher.AttachToTransition(transition);
        return transition;
    }



    
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ShouldCreateArcFromCapPlace_AndRestoreCapacity(int amountToConsume)
    {
        var from = CreateLocation(false);
        var to = CreateLocation(false);
        var transition = CreateAndAttachToTransition(from, to, amountToConsume,[]);
        
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().BeEmpty();
            
            transition.InGoing.First(e => e.From.IsCapacityLocation).Expression.ExpressionText
                .Should().Be($"{from.Capacity - amountToConsume}'{Colour.DefaultTokenColour}");
            
            transition.OutGoing.First(e => e.To.IsCapacityLocation).Expression.ExpressionText
                .Should().Be($"{from.Capacity}'{Colour.DefaultTokenColour}");
        }
    }
    
    [Fact]
    public void WhenConsumeIsEqualToCapacity_ShouldNotHaveArc_FromCapplace_ToTransition()
    {
        
        var from = CreateLocation(false);
        var to = CreateLocation(false);
        var transition = CreateAndAttachToTransition(from, to, from.Capacity,[]);
        
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().BeEmpty();
            transition.InGoing.Where(e => e.From.IsCapacityLocation).Should().BeEmpty();
            transition.OutGoing.First(e => e.To.IsCapacityLocation).Expression.ExpressionText
                .Should().Be($"{from.Capacity}'{Colour.DefaultTokenColour}");
        }
    }
    

    [Fact]
    public void ShouldHaveCorrectWeightFromCorrectArcs()
    {
        var from = CreateLocation(false);
        var to = CreateLocation(false);
        IEnumerable<Location> otherLocations = LocationGenerator.GenerateLocations(10).ToArray();
        var transition = CreateAndAttachToTransition(from, to, 10, otherLocations);
        
        var emptyBeforeNames = otherLocations.Select(l => l.Name);
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().AllSatisfy(e => e.Weight.Should().Be(1));
            transition.InhibitorArcs.Should().AllSatisfy(e => emptyBeforeNames.Should().Contain(e.From.Name));
        }
    }
    
    
}