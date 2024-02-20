using System.Collections.Generic;
using System.Linq;
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

[TestSubject(typeof(ConfigurationGenerator))]
public class ConfigurationGeneratorTest
{
    private const string PARTTYPE = "P1";
    private static readonly IEnumerable<string> PartTypes = [PARTTYPE]; 
    private LocationGenerator _locationGenerator = new(PartTypes);
    private LocationConfigurationFactory _configurationFactory = new(PartTypes);
    private ConfigurationGenerator GetGenerator(IEnumerable<MoveAction> actions)
    {
        return ConfigurationGenerator.WithDefaultImplementations(actions);
    }

    [Fact]
    public void WhenNoDelaysCanBeMade_ShouldReturnEmpty()
    {
        int min = 1; int max = 1;
        var location = CreateSingletonConfigWithAge(min, max, max);
        
        Configuration configurations = CreateConfiguration([location]);

        ReachableConfig[] reachableStates = GetGenerator([]).GenerateConfigurations(configurations);
        reachableStates.Should().BeEmpty();
    }



    [Theory]
    [InlineData(1,1)]
    [InlineData(0,1)]
    [InlineData(0,2)]
    [InlineData(3,4)]
    public void WhenOnlyOneDelayCanBeMade_ShouldReturnOneConfiguration_ReachableByDelay1(int min, int max)
    {
        var location = CreateSingletonConfigWithAge(min,max , max - 1);
        Configuration configurations = CreateConfiguration([location]);

        ReachableConfig[] reachableStates = GetGenerator([]).GenerateConfigurations(configurations);
        reachableStates.Should().HaveCount(1);
        reachableStates.First().TimeCost.Should().Be(1);
    }
    
    [Theory]
    [InlineData(1,3, 1)]
    [InlineData(0,3, 1)]
    [InlineData(0,2, 0)]
    public void WhenTwoDelaysCanBeMade_ShouldReturnTwoConfigurations(int min, int max, int age)
    {
        var location = CreateSingletonConfigWithAge(min, max, age);
        
        Configuration configurations = CreateConfiguration([location]);
        
        ReachableConfig[] reachableStates = GetGenerator([]).GenerateConfigurations(configurations);
        reachableStates.Should().HaveCount(2);
    }
    
    [Theory]
    [InlineData(0,3, 1, 2)] //Delay to reach 2,3
    [InlineData(1,3, 1, 2)] //Delay to reach age 2,3
    [InlineData(1,10, 1, 9)] //Delay to reach age 2,3,..,10
    [InlineData(1,10, 5, 5)] //Delay to reach age 6,7,8,9,10
    [InlineData(1,10, 10, 0)] //No delay possible
    public void WhenAgeBetweenInvariant_ShouldGiveExpectedNumberOfPossibleDelays(int min, int max, int age, int expected)
    {
        var location = CreateSingletonConfigWithAge(min, max, age);
        
        Configuration configurations = CreateConfiguration([location]);
        
        ReachableConfig[] reachableStates = GetGenerator([]).GenerateConfigurations(configurations);
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
        var location = CreateSingletonConfigWithAge(min, max, age);
        
        Configuration configurations = CreateConfiguration([location]);
        
        ReachableConfig[] reachableStates = GetGenerator([]).GenerateConfigurations(configurations);
        reachableStates.Should().HaveCount(expected);
    }
    
    
    

    private Configuration CreateConfiguration(IEnumerable<Location> locations)
    {
        ConfigurationFactory factory = new ConfigurationFactory();
        return factory.From(locations.Select(e=>(e,e.Configuration)));
    }
    
    private Location CreateSingletonConfigWithAge(int min, int max, int age)
    {
        return _locationGenerator.GetRegular() 
            with
            {
                Invariants = new HashSet<Invariant>([new Invariant(PARTTYPE, min, max)]),
                Configuration = _configurationFactory.SingletonOfAge(PARTTYPE, [], age)
            };
    }

}