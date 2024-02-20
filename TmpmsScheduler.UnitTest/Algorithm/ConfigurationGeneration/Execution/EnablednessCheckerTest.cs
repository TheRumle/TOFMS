using System.Collections.Generic;
using FluentAssertions;
using JetBrains.Annotations;
using TestDataGenerator;
using Tmpms;
using Tmpms.Factories;
using TmpmsChecker;
using TmpmsChecker.Algorithm;
using TmpmsChecker.Algorithm.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.Algorithm.ConfigurationGeneration.Execution;

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