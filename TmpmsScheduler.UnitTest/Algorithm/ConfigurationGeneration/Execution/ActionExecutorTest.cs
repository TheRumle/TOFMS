using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using TestDataGenerator;
using Tmpms;
using TmpmsChecker;
using TmpmsChecker.Algorithm;
using TmpmsChecker.Algorithm.ConfigurationGeneration;
using TmpmsChecker.Algorithm.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.Algorithm.ConfigurationGeneration.Execution;

[TestSubject(typeof(ActionExecutor))]
public class ActionExecutorTest
{
    private static readonly string[] partTypes = ["P1"];
    private static readonly LocationGenerator generator = new(partTypes);
    Location _from = generator.GetRegular() with{Name = "From"};
    Location _to = generator.GetRegular() with{Name = "To"};
    private IGenerator<Part> _partGenerator = new PartGenerator(partTypes, 0, 20);


    [Fact]
    public void WhenMovingFrom_AToB_AddsAndRemovesTheParts()
    {
        var parts = _partGenerator.Generate(2).ToArray();
        var consProd = new ConsumeProduceSet(parts, parts);
        
        ActionExecution execution = new ActionExecution([consProd]);
        ActionExecutor executor = new ActionExecutor();
        var configuration = CreateConfigurationAllEmpty(consProd);
        
        var newConfig = executor.Execute(_from, _to, execution,  configuration);
        newConfig.LocationConfigurations[_to].Size.Should().Be(2);
        newConfig.LocationConfigurations[_to].AllParts.SequenceEqual(parts).Should().Be(true);
        newConfig.LocationConfigurations[_from].Size.Should().Be(0);
    }
    
    private Configuration CreateConfigurationAllEmpty(ConsumeProduceSet consumeProduceSet)
    {
        LocationConfiguration fromConf = new LocationConfiguration(partTypes);
        fromConf.Add(consumeProduceSet.Consume); // We must have sufficient parts to perform the move
        
        LocationConfiguration toConf = new LocationConfiguration(partTypes);
        return new Configuration([KeyValuePair.Create(_to, toConf),KeyValuePair.Create(_from, fromConf)]);
    }
}