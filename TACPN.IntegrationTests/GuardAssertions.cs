using FluentAssertions;
using TACPN.Net;
using TACPN.Net.Places;

namespace TACPN.IntegrationTests;

public static class GuardAssertions
{
    public static void ShouldHaveLabels(this ColoredGuard guard, string color, int amount)
    {
        guard.ColourType.Name.ToLower().Should().Be(color);
        guard.Amount.Should().Be(amount);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }
    
    private static void ShouldHaveLabels(this ColoredGuard guard, string color, int amount, Interval interval)
    {
        guard.ColourType.Name.ToLower().Should().Be(color);
        guard.Amount.Should().Be(amount);
        guard.Interval.Should().Be(interval);
    }
    
    public static void ShouldHaveFirstColorIntervalBeDotWithZeroToInfinity(this ColoredGuard guard)
    {
        guard.ColourType.Should().Be(CapacityPlaceExtensions.DefaultCapacityColor);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }

    public static void ShouldBeOfColorAndHaveZeroToInfinity(this ColoredGuard guard, string color)
    {
        guard.ColourType.Should().Be(color);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }

    public static void InvariantShouldBe(this KeyValuePair<string, int> invariant, string name, int value )
    {
        invariant.Value.Should().Be(value);
        invariant.Key.ToLower().Should().Be(name);
    }
}

