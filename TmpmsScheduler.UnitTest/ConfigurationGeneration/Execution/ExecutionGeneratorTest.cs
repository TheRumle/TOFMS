using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using TestDataGenerator;
using Tmpms;
using Tmpms.Factories;
using Tmpms.Move;
using TmpmsChecker;
using TmpmsChecker.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.ConfigurationGeneration.Execution;

[TestSubject(typeof(ExecutionGenerator))]
public class ExecutionGeneratorTest
{
    [Theory]
    [InlineData(6,5)]
    [InlineData(2,1)]
    [InlineData(10,1)]
    public void WhenExecutingAction_WillExceedCapacity_ReturnsEmptyEnumerable(int parts, int capacity)
    {
        string part = "P1";
        var (from, to, configuration) = PrepareConfiguration(part, parts, capacity);
        MoveAction action = CreateMoveAction(part, parts, from, to);
        ExecutionGenerator decider = CreateDecider();
        decider.PossibleWaysToExecute(action, configuration).Should().HaveCount(0);
    }

    
    [Theory]
    [InlineData(1,1)]
    [InlineData(2,2)]
    [InlineData(10,14)]
    public void WhenExecutingAction_WillNotExceedCapacity_ReturnsNonEmptyEnumerable(int parts, int capacity)
    {
        string part = "P1";
        var (from, to, configuration) = PrepareConfiguration(part, parts, capacity);
        MoveAction action = CreateMoveAction(part, parts, from, to);
        ExecutionGenerator decider = CreateDecider();
        var possibleWaysToExecute = decider.PossibleWaysToExecute(action, configuration);
        
        possibleWaysToExecute.Should().HaveCountGreaterThan(0);
    }

    private (Location from, Location to, Configuration configuration) PrepareConfiguration(string part, int parts, int capacity)
    {
        LocationGenerator generator = new LocationGenerator(new List<string> { part });
        Location from = generator.GenerateSingle(ProcessingLocationStrategy.OnlyRegularLocations);
        Location to = generator.GenerateSingle(ProcessingLocationStrategy.OnlyRegularLocations) with { Capacity = capacity };
        
        ConfigurationFactory configurationFactory = new ConfigurationFactory();
        LocationConfigurationFactory locConfFactory = new LocationConfigurationFactory([part]);
        
        var configuration = configurationFactory.From(new List<(Location, LocationConfiguration)>
        {
            (from, CreateLocationConfigurationWithUniqueLengthJourney(part, parts, generator)),
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
            EmptyBefore = new HashSet<Location>(),
            EmptyAfter = new HashSet<Location>()
        };
    }

    private ExecutionGenerator CreateDecider()
    {
        return new ExecutionGenerator();
    }

    private LocationConfiguration CreateLocationConfigurationWithUniqueLengthJourney(string partType, int amount, LocationGenerator generator)
    {
        var jour = Enumerable.Repeat(generator.GetProcessing(), amount).ToArray();
        var a = new LocationConfiguration([partType]);
        for (var i = 0; i < amount; i++)
            a.Add(new Part(partType,0,jour.Skip(1)));

        return a;
    }
}