using FluentAssertions;
using TACPN.Net;
using TACPN.Net.Places;

namespace TACPN.IntegrationTests;

public static class GuardAssertions
{
    public static void ShouldHaveLabels(this ColourTimeGuard timeGuard, string color)
    {
        timeGuard.ColourType.Name.ToLower().Should().Be(color);
        timeGuard.Interval.Should().Be(Interval.ZeroToInfinity);
    }
    
    private static void ShouldHaveLabels(this ColourTimeGuard timeGuard, string color, int amount, Interval interval)
    {
        timeGuard.ColourType.Name.ToLower().Should().Be(color);
        timeGuard.Interval.Should().Be(interval);
    }
    
    public static void ShouldHaveFirstColorIntervalBeDotWithZeroToInfinity(this ColourTimeGuard timeGuard)
    {
        timeGuard.ColourType.Should().Be(CapacityPlaceExtensions.DefaultCapacityColor);
        timeGuard.Interval.Should().Be(Interval.ZeroToInfinity);
    }

    public static void ShouldBeOfColorAndHaveZeroToInfinity(this ColourTimeGuard timeGuard, string color)
    {
        timeGuard.ColourType.Should().Be(color);
        timeGuard.Interval.Should().Be(Interval.ZeroToInfinity);
    }

    public static void InvariantShouldBe(this KeyValuePair<string, int> invariant, string name, int value )
    {
        invariant.Value.Should().Be(value);
        invariant.Key.ToLower().Should().Be(name);
    }
}

