using FluentAssertions;
using TACPN.Net;

namespace TACPN.IntegrationTests;

public static class ColoredGuardAssertions
{
    public static void ShouldHaveFirstColorIntervalBeDotWithZeroToInfinity(this ColoredGuard guard)
    {
        var dotGuard = guard.ColorIntervals.FirstOrDefault(e=> e.Key == CapacityPlaceCreator.DefaultColorName);
        dotGuard.Should().NotBeNull("guards for dots must have guard with 'dot' key");
        dotGuard.Value.Should().Be(Interval.ZeroToInfinity, "");
    }
    
    public static void ShouldBeOfColorAndHaveZeroToInfinity(this ColoredGuard guard, string color)
    {
        guard.ColorIntervals.First().Key.Should().Be(color);
        guard.ColorIntervals.First().Value.Should().Be(Interval.ZeroToInfinity);
    }
}