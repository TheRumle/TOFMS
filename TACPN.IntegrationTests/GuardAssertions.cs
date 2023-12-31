﻿using FluentAssertions;
using TACPN.Net;

namespace TACPN.IntegrationTests;

public static class GuardAssertions
{
    public static void ShouldHaveLabels(this ColoredGuard guard, string color, int amount)
    {
        guard.Color.Name.ToLower().Should().Be(color);
        guard.Amount.Should().Be(amount);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }
    
    private static void ShouldHaveLabels(this ColoredGuard guard, string color, int amount, Interval interval)
    {
        guard.Color.Name.ToLower().Should().Be(color);
        guard.Amount.Should().Be(amount);
        guard.Interval.Should().Be(interval);
    }
    
    public static void ShouldHaveFirstColorIntervalBeDotWithZeroToInfinity(this ColoredGuard guard)
    {
        guard.Color.Should().Be(CapacityPlaceExtensions.DefaultCapacityColor);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }

    public static void ShouldBeOfColorAndHaveZeroToInfinity(this ColoredGuard guard, string color)
    {
        guard.Color.Should().Be(color);
        guard.Interval.Should().Be(Interval.ZeroToInfinity);
    }

    public static void InvariantShouldBe(this KeyValuePair<string, int> invariant, string name, int value )
    {
        invariant.Value.Should().Be(value);
        invariant.Key.ToLower().Should().Be(name);
    }
}

