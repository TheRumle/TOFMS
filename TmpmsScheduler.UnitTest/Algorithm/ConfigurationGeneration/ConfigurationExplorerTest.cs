using System;
using System.Collections.Generic;
using System.Linq;
using Common.DiscreteMathematics;
using FluentAssertions;
using JetBrains.Annotations;
using TestDataGenerator;
using Tmpms;
using Tmpms.Factories;
using Tmpms.Move;
using TmpmsChecker.Algorithm;
using TmpmsChecker.Algorithm.ConfigurationGeneration;
using Xunit;

namespace TmpmsScheduler.UnitTest.Algorithm.ConfigurationGeneration;

[TestSubject(typeof(ConfigurationExplorer))]
public class ConfigurationExplorerTest
{
    private const string PARTTYPE = "P1";
    private static readonly IEnumerable<string> PartTypes = [PARTTYPE]; 
    private LocationGenerator _locationGenerator = new(PartTypes);
    private LocationConfigurationFactory _configurationFactory = new(PartTypes);
    private Random _random = new Random();

    
    private ConfigurationExplorer GetConfigurationExplorer(IEnumerable<MoveAction> actions)
    {
        return ConfigurationExplorer.WithDefaultImplementations(actions);
    }

    [Fact]
    public void WhenOnlyOneWayToExecuteAction_ShouldGiveOnePossibility()
    {
        var to = CreateToLocation(true);
        var from = CreateLocationWithEnabledPartMovingInto(10,to);
        var action = CreateMoveAction(from, to, from.Configuration.PartsByType);
        
        Configuration systemConfiguration = CreateConfiguration([from,to]);

        var explorer = GetConfigurationExplorer([action]);
        var result = explorer.GenerateConfigurations(systemConfiguration);
        result.Should().HaveCount(1);
    }
    
    [Fact]
    public void WhenTwoWaysToExecuteAction_ShouldGiveTwoPossibility()
    {
        var to = CreateToLocation(true);
        var from = CreateLocationWithEnabledPartsOfAges([3,5], to);
        
        var action = CreateMoveAction(from, to, 1);
        
        Configuration systemConfiguration = CreateConfiguration([from,to]);

        var explorer = GetConfigurationExplorer([action]);
        var result = explorer.GenerateConfigurations(systemConfiguration);
        result.Should().HaveCount(2);
    }
    
    [Fact]
    public void WhenThreeWaysToExecuteAction_ShouldGiveThreePossibility()
    {
        var to = CreateToLocation(true);
        var from = CreateLocationWithEnabledPartsOfAges([3,5,9], to);
        
        var action = CreateMoveAction(from, to, 1);
        
        Configuration systemConfiguration = CreateConfiguration([from,to]);

        var explorer = GetConfigurationExplorer([action]);
        var result = explorer.GenerateConfigurations(systemConfiguration);
        result.Should().HaveCount(3);
    }
    
    
    [Theory]
    [InlineData(3,9)]
    public void WhenTwoWaysToExecuteAction_AndOneDelayIsPossible_ShouldGiveTwoPossibility(int min, int max)
    {
        var to = CreateToLocation(true);
        
        var from = CreateLocationWithInvariantBuffer([min,max], to, 1);
        from.Configuration.Add(new Part(PARTTYPE, min-1, [to]));
        var action = CreateMoveAction(from, to, 1);
        
        Configuration systemConfiguration = CreateConfiguration([from,to]);

        var explorer = GetConfigurationExplorer([action]);
        var result = explorer.GenerateConfigurations(systemConfiguration);
        result.Should().HaveCount(3);
    }
    
    
    /// <summary> Expected calculated using nCr = n! / r! (n - r)!
    /// See <see cref="PermutationsAndCombinations.NumberOfCombinationsOfSize"/> for naive implementation.
    /// </summary>
    /// <param name="numberParts"></param>
    /// <param name="neededForAction"></param>
    [Theory]
    [InlineData(10, 2, 45)]
    [InlineData(11, 10,11)]
    [InlineData(20, 7, 77520)]
    [InlineData(3, 2, 3)]
    [InlineData(30, 2,435)]
    public void WhenNeedingMultipleParts_ShouldCreateAllPermutations(int numberParts, int neededForAction, int expected)
    {
        var partAges = Enumerable.Range(0, numberParts).ToArray();
        
        var to = CreateToLocation(true);
        var from = CreateLocationWithEnabledPartsOfAges(partAges, to);
        
        var action = CreateMoveAction(from, to, neededForAction);
        
        Configuration systemConfiguration = CreateConfiguration([from,to]);

        var explorer = GetConfigurationExplorer([action]);
        var result = explorer.GenerateConfigurations(systemConfiguration);
        result.Should().HaveCount(expected);
    }
    
    [Fact]
    public void WhenNoDelaysCanBeMade_ShouldReturnEmpty()
    {
        int min = 1; int max = 1;
        var location = CreateSingletonConfigWithInvariantAndAge(min, max, max);
        
        Configuration configurations = CreateConfiguration([location]);

        ReachableConfig[] reachableStates = GetConfigurationExplorer([]).GenerateConfigurations(configurations);
        reachableStates.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1,1)]
    [InlineData(0,1)]
    [InlineData(0,2)]
    [InlineData(3,4)]
    public void WhenOnlyOneDelayCanBeMade_ShouldReturnOneConfiguration_ReachableByDelay1(int min, int max)
    {
        var location = CreateSingletonConfigWithInvariantAndAge(min,max , max - 1);
        Configuration configurations = CreateConfiguration([location]);

        ReachableConfig[] reachableStates = GetConfigurationExplorer([]).GenerateConfigurations(configurations);
        reachableStates.Should().HaveCount(1);
        reachableStates.First().TimeCost.Should().Be(1);
    }
    
    [Theory]
    [InlineData(0,3, 1, 2)] //Delay to reach 2,3
    [InlineData(1,3, 1, 2)] //Delay to reach age 2,3
    [InlineData(1,10, 1, 9)] //Delay to reach age 2,3,..,10
    [InlineData(1,10, 5, 5)] //Delay to reach age 6,7,8,9,10
    [InlineData(1,10, 10, 0)] //No delay possible
    public void WhenAgeBetweenInvariant_ShouldGiveExpectedNumberOfPossibleDelays(int min, int max, int age, int expected)
    {
        var location = CreateSingletonConfigWithInvariantAndAge(min, max, age);
        
        Configuration configurations = CreateConfiguration([location]);
        
        ReachableConfig[] reachableStates = GetConfigurationExplorer([]).GenerateConfigurations(configurations);
        reachableStates.Should().HaveCount(expected);
    }
    
    [Theory]
    [InlineData(5,10, 0, 6)] //Delay to reach 5,6,7,8,9,10
    [InlineData(5,10, 1, 6)] //Delay to reach 5,6,7,8,9,10
    [InlineData(5,10, 2, 6)] //Delay to reach 5,6,7,8,9,10
    [InlineData(5,10, 4, 6)] //Delay to reach 5,6,7,8,9,10
    [InlineData(5,10, 5, 5)] //Delay to reach 6,7,8,9,10
    [InlineData(10,10, 0, 1)] //Delay to reach 10
    public void WhenAgeUnderInvariant_ShouldGiveExpectedNumberOfPossibleDelays(int min, int max, int age, int expected)
    {
        var location = CreateSingletonConfigWithInvariantAndAge(min, max, age);
        
        Configuration configurations = CreateConfiguration([location]);
        
        ReachableConfig[] reachableStates = GetConfigurationExplorer([]).GenerateConfigurations(configurations);
        reachableStates.Should().HaveCount(expected);
    }
    
    
    

    private Configuration CreateConfiguration(IEnumerable<Location> locations)
    {
        ConfigurationFactory factory = new ConfigurationFactory();
        return factory.From(locations.Select(e=>(e,e.Configuration)));
    }
    
    private Location CreateSingletonConfigWithInvariantAndAge(int min, int max, int age)
    {
        return _locationGenerator.GetRegular() 
            with
            {
                Invariants = new HashSet<Invariant>([new Invariant(PARTTYPE, min, max)]),
                Configuration = _configurationFactory.SingletonOfAge(PARTTYPE, [], age)
            };
    }


    
    private MoveAction CreateMoveAction(Location from, Location to, IEnumerable<Part> toMove)
    {
        return new MoveAction()
        {
            From = from,
            EmptyAfter = new HashSet<Location>(),
            EmptyBefore = new HashSet<Location>(),
            Name = "Test",
            To = to,
            PartsToMove = toMove
                .GroupBy(part => part.PartType) // Group parts by PartType
                .ToDictionary(
                        group => group.Key, // Key is the PartType
                        group => group.AsEnumerable().Count()
                    )
        };
    }
    
    private MoveAction CreateMoveAction(Location from, Location to, IReadOnlyDictionary<string, List<Part>> toMove)
    {
        return CreateMoveAction(from, to, toMove.Values.SelectMany(e=>e));
    }

    private Location CreateToLocation(bool isProccessing)
    {
        return _locationGenerator.GetProcessing() with
        {
            Invariants = new HashSet<Invariant>()
            {
                new(PARTTYPE, 0, 200)
            },
            IsProcessing = isProccessing,
            Capacity = 9999
        };
    }

    private Location CreateLocationWithEnabledPartMovingInto(int age, Location to)
    {
        Location[] jour = to.IsProcessing ? [to] : [];
        return _locationGenerator.GetRegular() 
            with
            {
                Invariants = new HashSet<Invariant>([new Invariant(PARTTYPE, age,age)]),
                Configuration = _configurationFactory.SingletonOfAge(PARTTYPE, jour, age)
            };
    }

    private Location CreateLocationWithEnabledPartsOfAges(int[] ages, Location to)
    {
        Location[] jour = to.IsProcessing ? [to] : [];
        var tuples = ages.Select(age => (PARTTYPE, jour, e: age));
        return _locationGenerator.GetRegular() 
            with
            {
                Invariants = new HashSet<Invariant>([new Invariant(PARTTYPE, ages.Min(), ages.Max())]),
                Configuration = _configurationFactory.OfValues(tuples)
            };
    }
    
    private MoveAction CreateMoveAction(Location from, Location to, int partsNeeded)
    {
        IEnumerable<Location> jour = to.IsProcessing ? [to] : [];
        return CreateMoveAction(from, to, Enumerable.Repeat(new Part(PARTTYPE, 0, jour), partsNeeded));
    }
    
    private Location CreateLocationWithInvariantBuffer(int[] ages, Location to, int timeBufferSize)
    {
        Location[] jour = to.IsProcessing ? [to] : [];
        IEnumerable<(string PARTTYPE, Location[] jour, int e)> tuples = ages.Select(age => (PARTTYPE, jour, e: age));

        return _locationGenerator.GetRegular() 
            with
            {
                Invariants = new HashSet<Invariant>([new Invariant(PARTTYPE, ages.Min(), ages.Max()+timeBufferSize)]),
                Configuration = _configurationFactory.OfValues(tuples)
            };
    }


}