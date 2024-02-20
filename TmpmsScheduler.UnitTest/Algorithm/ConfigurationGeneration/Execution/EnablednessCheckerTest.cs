using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using TestDataGenerator;
using Tmpms;
using Tmpms.Factories;
using Tmpms.Move;
using TmpmsChecker;
using TmpmsChecker.Algorithm.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.Algorithm.ConfigurationGeneration.Execution;

public class SinglePartMoveTest
{
      
    public (Location from, Location to, Configuration configuration) PrepareConfiguration(string part, int parts, int capacity)
    {
        LocationGenerator generator = new LocationGenerator(new List<string> { part });
        Location from = generator.GetRegular();
        Location to = generator.GetRegular() with { Capacity = capacity };
        
        ConfigurationFactory configurationFactory = new ConfigurationFactory();
        LocationConfigurationFactory locConfFactory = new LocationConfigurationFactory([part]);
        
        var configuration = configurationFactory.From(new List<(Location, LocationConfiguration)>
        {
            (from, CreateLocationConfigurationWithUniqueLengthJourney(part, parts, generator)),
            (to, locConfFactory.Empty())
        });

        return (from, to, configuration);
    }

    public MoveAction CreateMoveAction(string part, int parts, Location from, Location to)
    {
        return CreateMoveAction(part, parts, from, to, [], []);
    }
    
    public MoveAction CreateMoveAction(string part, int parts, Location from, Location to, HashSet<Location> emptyAfter, HashSet<Location> emptyBefore)
    {
        return new MoveAction()
        {
            Name = "Test",
            From = from,
            To = to,
            PartsToMove = new Dictionary<string, int> { { part, parts } },
            EmptyBefore = emptyBefore,
            EmptyAfter = emptyAfter
        };
    }


    public LocationConfiguration CreateLocationConfigurationWithUniqueLengthJourney(string partType, int amount, LocationGenerator generator)
    {
        var jour = Enumerable.Repeat(generator.GetProcessing(), amount).ToArray();
        var a = new LocationConfiguration([partType]);
        for (var i = 0; i < amount; i++)
            a.Add(new Part(partType,0,jour.Skip(1)));

        return a;
    }
}

[TestSubject(typeof(EnablednessChecker))]
public class EnablednessCheckerTest : SinglePartMoveTest
{
    private const string Part = "P1";
    private static readonly List<string> Parts = [Part];
    private LocationGenerator _locationGenerator = new(Parts);
    
    [Fact]
    public void WhenCapacityIsInsufficient_ShouldGiveFalse()
    {
        int capacity = 1;
        int numberOfParts = 2;
        (Location from, Location to, Configuration configuration) = PrepareConfiguration(Part, numberOfParts, capacity);
        var action = CreateMoveAction(Part, numberOfParts, from, to with{ Capacity = capacity});
        action.SatisfiesBaseEnabledness(configuration).Should().Be(false);
    }
    
    [Fact]
    public void When_EmptyBeforeContainsNonEmptyLocation_ShouldGiveFalse()
    {
        int capacity = 10;
        int numberOfParts = 2;

        var bef = _locationGenerator.GenerateSingle();
        LocationConfigurationFactory factory = new LocationConfigurationFactory(Parts);
        (Location from, Location to, Configuration configuration) = PrepareConfiguration(Part, numberOfParts, capacity);
        configuration.Add(bef,factory.ZeroAgedOfPartType(Part, []));
        
        var action = CreateMoveAction(Part, numberOfParts, from, to with{ Capacity = capacity}, [], [bef]);
        action.SatisfiesBaseEnabledness(configuration).Should().Be(false);
    }
    
    [Fact]
    public void When_EmptyAfterContains_NonEmptyLocationNotAffected_ShouldGiveFalse()
    {
        int capacity = 10;
        int numberOfParts = 2;

        var shouldBeEmptyAfter = _locationGenerator.GenerateSingle();
        LocationConfigurationFactory factory = new LocationConfigurationFactory(Parts);
        (Location from, Location to, Configuration configuration) = PrepareConfiguration(Part, numberOfParts, capacity);
        configuration.Add(shouldBeEmptyAfter,factory.ZeroAgedOfPartType(Part, []));
        
        var action = CreateMoveAction(Part, numberOfParts, from, to with{ Capacity = capacity}, [shouldBeEmptyAfter], []);
        action.SatisfiesBaseEnabledness(configuration).Should().Be(false);
    }
    
    [Fact]
    public void When_EmptyAfterContainsFrom_FromContainsJustEnoughParts_ShouldReturnTrue()
    {
        int capacity = 10;
        int numberOfParts = 2;

        (Location from, Location to, Configuration configuration) = PrepareConfiguration(Part, numberOfParts, capacity);
        
        var action = CreateMoveAction(Part, numberOfParts, from, to with{ Capacity = capacity}, [], []);
        action.SatisfiesBaseEnabledness(configuration).Should().Be(true);
    }
    
    [Fact]
    public void When_EmptyAfterContainsTo_ToContainsExcessiveParts_ShouldReturnFalse()
    {
        int capacity = 10;
        int numberOfParts = 2;

        (Location from, Location to, Configuration configuration) = PrepareConfiguration(Part, numberOfParts+1, capacity);
        
        var action = CreateMoveAction(Part, numberOfParts, from, to with{ Capacity = capacity}, [], []);
        action.SatisfiesBaseEnabledness(configuration).Should().Be(true);
    }
    
  
}