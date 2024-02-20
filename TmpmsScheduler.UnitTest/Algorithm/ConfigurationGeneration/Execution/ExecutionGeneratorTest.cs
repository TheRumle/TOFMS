using FluentAssertions;
using JetBrains.Annotations;
using Tmpms.Move;
using TmpmsChecker.Algorithm.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.Algorithm.ConfigurationGeneration.Execution;

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
 
    private ExecutionGenerator CreateDecider()
    {
        return new ExecutionGenerator();
    }
}