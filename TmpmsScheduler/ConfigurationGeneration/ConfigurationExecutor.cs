using Tmpms;
using Tmpms.Move;
using TmpmsChecker.Algorithm;
using TmpmsChecker.Domain;
using MoveAction = TmpmsChecker.Domain.MoveAction;

namespace TmpmsChecker.ConfigurationGeneration;

internal class ConfigurationExecutor : IConfigurationGenerator
{
    private readonly IEnumerable<MoveAction> _availableActions;
    private readonly IActionExecutionGenerator _executionGenerator;
    private readonly IActionExecutor _actionExecutor;

    public ConfigurationExecutor(
        IEnumerable<MoveAction> availableActions,
        IActionExecutionGenerator executionGenerator,
        IActionExecutor actionExecutor)
    {
        _availableActions = availableActions;
        _executionGenerator = executionGenerator;
        _actionExecutor = actionExecutor;
    }
    
    public IEnumerable<ReachedState> GenerateConfigurations(Configuration configuration)
    {
        return _availableActions
            .AsParallel().WithDegreeOfParallelism(4)
            .Select(action => (
                Action: action,
                PossibleWaysToExecute: _executionGenerator.PossibleWaysToExecute(action,  under: configuration))
            )
            .SelectMany(actionPossibilities =>
            {
                return actionPossibilities.PossibleWaysToExecute
                    .Select(wayToExecute => _actionExecutor.Execute(wayToExecute, configuration))
                    .Select(reachedConfiguration => new ReachedState(actionPossibilities.Action, reachedConfiguration));
            })
            .Concat(ComputePossibleDelays(configuration).AsParallel());
    }

    private IEnumerable<ReachedState> ComputePossibleDelays(Configuration configuration)
    {
        return PossibleDelaySpan(configuration) switch
        {
            (_, <= 0) => [],
            var (minimumTime, maxPossibleDelay) =>
                Enumerable
                    .Range(minimumTime, minimumTime - maxPossibleDelay)
                    .Select(delay => new ReachedState(delay, _actionExecutor.Delay(delay, configuration)))
        };
    }

    private (int TimeUntilMinReached, int MaxDelay) PossibleDelaySpan(Configuration configuration)
    {
        var maxDelay = 0;
        var timeUntilMinReached = 0;
        foreach (var (location, locationConfig) in configuration.LocationConfigurations)
            foreach (var (partType, invariant) in location.InvariantsByType)
                foreach (var part in locationConfig.PartsByType[partType])
                {
                    //If any part is at the max we cannot perform any delay
                    if (invariant.Max == part.Age) return (0, 0);
                    maxDelay = Math.Min(maxDelay, invariant.Max - part.Age);
                    timeUntilMinReached = Math.Min(timeUntilMinReached, invariant.Min - part.Age);
                }
        return (timeUntilMinReached, maxDelay);
    }
}