using FluentAssertions;
using JetBrains.Annotations;
using Tmpms.Move;
using TmpmsChecker.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.ConfigurationGeneration.Execution;

[TestSubject(typeof(ExecutionGenerator))]
public class ExecutionGeneratorTest : SinglePartMoveTest
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
    
    private ExecutionGenerator CreateDecider()
    {
        return new ExecutionGenerator();
    }
}