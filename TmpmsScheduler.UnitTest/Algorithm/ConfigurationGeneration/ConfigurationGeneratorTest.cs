using System.Collections;
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

    
    [Theory]
    [InlineData(1,1)]
    [InlineData(0,1)]
    [InlineData(0,2)]
    public void WhenOnlyDelayCanBeMade_ShouldReturnOneConfiguration_ReachableByDelay(int min, int max)
    {
        var location = _locationGenerator.GetRegular() 
            with
            {
                Invariants = new HashSet<Invariant>([new Invariant(PARTTYPE, min, max)]),
                Configuration = _configurationFactory.SingletonOfAge(PARTTYPE, [], max-1)
            };
        
        
        Configuration configurations = CreateConfiguration([location]);

        ReachableConfig[] reachableStates = GetGenerator([]).GenerateConfigurations(configurations);
        reachableStates.Should().HaveCount(1);
        reachableStates.First().TimeCost().Should().Be(1);
    }

    private Configuration CreateConfiguration(IEnumerable<Location> locations)
    {
        ConfigurationFactory factory = new ConfigurationFactory();
        return factory.From(locations.Select(e=>(e,e.Configuration)));
    }
}