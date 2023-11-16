using FluentAssertions;
using TACPN.Net;

namespace TACPN.IntegrationTests;

public static class ColoredGuardAssertions
{
    public static void ShouldHaveFirstColorIntervalBeDotWithZeroToInfinity(this ColoredGuard guard)
    {
        guard.Color.Should().Be(CapacityPlaceCreator.DefaultColorName);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }
    
    public static void ShouldBeOfColorAndHaveZeroToInfinity(this ColoredGuard guard, string color)
    {
        guard.Color.Should().Be(color);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }
}