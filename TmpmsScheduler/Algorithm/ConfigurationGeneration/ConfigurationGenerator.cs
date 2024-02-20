﻿using Tmpms.Move;
using TmpmsChecker.Algorithm.ConfigurationGeneration.Execution;

namespace TmpmsChecker.Algorithm.ConfigurationGeneration;

internal class ConfigurationGenerator : IConfigurationGenerator
{
    private readonly IEnumerable<MoveAction> _availableActions;
    private readonly IActionExecutionGenerator _executionGenerator;
    private readonly IActionExecutor _actionExecutor;

    public static ConfigurationGenerator WithDefaultImplementations(IEnumerable<MoveAction> availableActions)
    {
        return new ConfigurationGenerator(availableActions,new ExecutionGenerator() ,new ActionExecutor());
    }

    public ConfigurationGenerator(
        IEnumerable<MoveAction> availableActions,
        IActionExecutionGenerator executionGenerator,
        IActionExecutor actionExecutor)
    {
        _availableActions = availableActions;
        _executionGenerator = executionGenerator;
        _actionExecutor = actionExecutor;
    }
    
    public ReachableConfig[] GenerateConfigurations(Configuration configuration)
    {
        return _availableActions
            .AsParallel().WithDegreeOfParallelism(4)
            .Select(action => (
                Action: action,
                PossibleWaysToExecute: _executionGenerator.PossibleWaysToExecute(action,  under: configuration))
            )
            .SelectMany(actionPossibilities =>
            {
                var action = actionPossibilities.Action;
                var possibleWaysToExecute = actionPossibilities.PossibleWaysToExecute;
                return possibleWaysToExecute
                    .Select(wayToExecute => _actionExecutor.Execute(action.From, action.To, wayToExecute, configuration))
                    .Select(reachedConfiguration => new ReachableConfig(actionPossibilities.Action, reachedConfiguration));
            })
            .Concat(ComputePossibleDelays(configuration).AsParallel())
            .ToArray();
    }

    private IEnumerable<ReachableConfig> ComputePossibleDelays(Configuration configuration)
    {
        return PossibleDelaySpan(configuration) switch
        {
            (_, <= 0) => [],
            var (minimumTime, maxPossibleDelay) =>
                Enumerable
                    .Range(minimumTime, minimumTime - maxPossibleDelay)
                    .Select(delay => new ReachableConfig(delay, _actionExecutor.Delay(delay, configuration)))
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