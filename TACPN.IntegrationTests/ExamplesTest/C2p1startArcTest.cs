using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures.Tofms.Fixtures;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Tofms.Common.Move;
using Xunit;

namespace TACPN.IntegrationTests.ExamplesTest;

public class C2P1StartArcTest : ComponentTest, IClassFixture<CentrifugeFixture>
{
    private static readonly int AmountToMove = 2;

    public C2P1StartArcTest(CentrifugeFixture fixture) : base(fixture.ComponentText)
    {
    }
    
    
    [Fact]
    public async Task ShouldHave_OneTransition_With_TwoIngoing_TwoOutgoing_OneInhibitor()
    {
        MoveAction action = await GetAction();
        var net = await Translater.TranslateAsync(action);

        net.Transitions.Should().HaveCount(1);
        var transition = net.Transitions.First();
        transition.InGoing.Should().HaveCount(3);
        transition.OutGoing.Should().HaveCount(2);
        transition.InhibitorArcs.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_2p1InfiniteAge_From_CBuffer()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstIngoingFromPlaceContaining("cbuffer");
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels("p1", AmountToMove);
        }
    }
    
    [Fact]
    public async Task ShouldHave_InhibitorArc_With_Weight1_From_CProc()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstInhibitorFromPlaceWithName("cproc");
            arc.Should().NotBeNull();
            arc!.Weight.Should().Be(1);
        }
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CProcHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.InGoing.First(e =>
                e.From.Name.ToLower().Contains("cproc") && e.From.Name.ToLower().Contains(Hat));
            
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels(dot, AmountToMove);
        }
    }
    
    //[Fact] //We now use inhibitor arc instead
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CBufferHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            IngoingArc arc = transition.FindFirstIngoingFromPlaceContaining("cbuffer", Hat);
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels(dot, 4 - AmountToMove - 1);
        }
    }
    
    [Fact]
    public async Task ShouldHave_OutGoing_With_2P1_To_CProc()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstOutgoingToPlaceWithName("cproc");
            arc.Productions.Should().HaveCount(1);
            Production production = arc.Productions.First();
            production.Color.ToLower().Should().Be("p1");
            production.Amount.Should().Be(AmountToMove);
        }
    }
    
    [Fact]
    public async Task ShouldHave_OutGoing_With_2Dot_To_CBufferHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.OutGoing.First(e =>
                e.To.IsCapacityLocation && e.To.Name.ToLower().Contains("cbuffer"));
            arc.Productions.Should().HaveCount(1);
            Production production = arc.Productions.First();
            production.Color.Should().Be(dot);
            production.Amount.Should().Be(4);
        }
    }

    [Fact]
    public async Task FromHatShouldHave_IngoingArc_WithWeight_CapacityMinusSumConsumeMinusOne()
    {
        Transition transition = await GetFirstTransition();
        using (new AssertionScope())
        {
            var guards = transition.InGoing.First(e=>e.From.IsCapacityLocation && e.From.Name.ToLower().Contains("cbuffer")).Guards;
            guards.Should().NotBeNull();
            guards.Select(e=>e.Amount).Sum().Should().Be(4 - AmountToMove);
        }
    }
    
}