using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures.Fixtures;
using TACPN.Net.Transitions;
using Tofms.Common;
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