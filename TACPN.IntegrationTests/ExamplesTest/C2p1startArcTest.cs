using Common;
using Common.Move;
using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures.Fixtures;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Xunit;

namespace TACPN.IntegrationTests.ExamplesTest;

public class C2P1StartPlacesTest : ComponentTest, IClassFixture<CentrifugeFixture>
{
    public C2P1StartPlacesTest(CentrifugeFixture fixture) : base(fixture.ComponentText)
    {
    }

    [Fact]
    public async Task CProcShouldHave_InvariantP1LessThan_12()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var place = transition.FindFirstConnectedPlaceWithName("cproc");
            var invariant = place.ColorInvariants.First(e => e.Key.ToLower().Contains("p1"));
            place.ColorInvariants.First().InvariantShouldBe("p1", 12);
        }
    }
    
    [Fact]
    public async Task CbufferShouldHave_InvariantP1LessThan_Infinity()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var place = transition.FindFirstConnectedPlaceWithName("cbuffer");
            var invariant = place.ColorInvariants.First(e => e.Key.ToLower().Contains("p1"));
            invariant.InvariantShouldBe("p1", InfinityInteger.Positive);
        }
    }
    
    [Fact]
    public async Task CProcCapacity_ShouldHave_InvariantDotInfinity()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var place = transition.FindFirstConnectedPlaceWithName("cbuffer",Hat);
            place.ColorInvariants.Should().HaveCount(1);
            place.ColorInvariants.First().InvariantShouldBe(dot, InfinityInteger.Positive);
        }
    }
    
    [Fact]
    public async Task BufferCapacity_ShouldHave_InvariantDotInfinity()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var place = transition.FindFirstConnectedPlaceWithName("buffer",Hat);
            place.ColorInvariants.Should().HaveCount(1);
            place.ColorInvariants.First().InvariantShouldBe(dot, InfinityInteger.Positive);
        }
    }
    
}

public class C2P1StartArcTest : ComponentTest, IClassFixture<CentrifugeFixture>
{
    public C2P1StartArcTest(CentrifugeFixture fixture) : base(fixture.ComponentText)
    {
    }
    

    [Fact]
    public async Task ShouldHave_OneTransition_With_ThreeIngoing_TwoOutgoing_OneInhibitor()
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
            arc.Guards.First().ShouldHaveLabels("p1", 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_InhibitorArc_With_Weight1_From_CProc()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstInhibitorFromPlaceContaining("cproc");
            arc.Amounts.Should().HaveCount(1);
        }
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CProcHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstIngoingFromPlaceContaining("cproc", Hat);
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CBufferHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            IngoingArc arc = transition.FindFirstIngoingFromPlaceContaining("cbuffer", Hat);
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_OutGoing_With_2P1_To_CProc()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstOutgoingToPlaceContaining("cproc");
            arc.Productions.Should().HaveCount(1);
            Production production = arc.Productions.First();
            production.Color.ToLower().Should().Be("p1");
            production.Amount.Should().Be(2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_OutGoing_With_2Dot_To_BufferHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstOutgoingToPlaceContaining("cbuffer", Hat);
            arc.Productions.Should().HaveCount(1);
            Production production = arc.Productions.First();
            production.Color.Should().Be(dot);
            production.Amount.Should().Be(2);
        }
    }

    
}