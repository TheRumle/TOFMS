using System.Collections.Generic;
using FluentAssertions;
using JetBrains.Annotations;
using TestDataGenerator;
using Tmpms;
using Tmpms.Factories;
using Tmpms.Move;
using TmpmsChecker.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.ConfigurationGeneration.Execution;

[TestSubject(typeof(EnablednessDecider))]
public class EnablednessDeciderTest
{
    public EnablednessDeciderTest()
    {
        
    }
    [Theory]
    [InlineData(6,5)]
    [InlineData(2,1)]
    [InlineData(10,1)]
    public void WhenExecutingAction_WillExceedCapacity_ReturnsFalse(int parts, int capacity)
    {
        string part = "P1";
        var (from, to, configuration) = PrepareConfiguration(part, parts, capacity);
        MoveAction action = CreateMoveAction(part, parts, from, to);
        EnablednessDecider decider = CreateDecider();
        decider.IsEnabledUnder(action, configuration).Should().Be(false);
    }

    
    [Theory]
    [InlineData(1,1)]
    [InlineData(2,2)]
    [InlineData(10,14)]
    public void WhenExecutingAction_WillNotExceedCapacity_ReturnsTrue(int parts, int capacity)
    {
        string part = "P1";
        var (from, to, configuration) = PrepareConfiguration(part, parts, capacity);
        MoveAction action = CreateMoveAction(part, parts, from, to);
        EnablednessDecider decider = CreateDecider();
        decider.IsEnabledUnder(action, configuration).Should().Be(true);
    }

    private (Location from, Location to, Configuration configuration) PrepareConfiguration(string part, int parts, int capacity)
    {
        LocationGenerator generator = new LocationGenerator(new List<string> { part });
        Location from = generator.GenerateSingle(ProcessingLocationStrategy.OnlyRegularLocations);
        Location to = generator.GenerateSingle(ProcessingLocationStrategy.OnlyRegularLocations) with { Capacity = capacity };
        ConfigurationFactory configurationFactory = new ConfigurationFactory();
        LocationConfigurationFactory locConfFactory = new LocationConfigurationFactory(new List<string> { part });

        var configuration = configurationFactory.From(new List<(Location, LocationConfiguration)>
        {
            (from, locConfFactory.ZeroAgedOfPart(part, [], parts)),
            (to, locConfFactory.Empty())
        });

        return (from, to, configuration);
    }

    private MoveAction CreateMoveAction(string part, int parts, Location from, Location to)
    {
        return new MoveAction()
        {
            Name = "Test",
            From = from,
            To = to,
            PartsToMove = new Dictionary<string, int> { { part, parts } },
            EmptyBefore = {},
            EmptyAfter = {}
        };
    }

    private EnablednessDecider CreateDecider()
    {
        return new EnablednessDecider(); // Assuming this is how you create an EnablednessDecider
    }
}