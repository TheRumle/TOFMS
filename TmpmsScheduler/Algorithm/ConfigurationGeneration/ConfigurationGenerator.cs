using Tmpms.Move;
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
            (<0,_) => throw new ArgumentException($"Min delay can never be smaller than 0."),
            (_,<0) => throw new ArgumentException($"Max delay can never be smaller than 0."),
            (_, 0 ) => [],
            (_, 1) => [CreateSingle(1, configuration)],
            (0, var maxDelay ) => CreateRange(1, maxDelay, configuration),
            (var minDelay, var maxDelay) => CreateRange(minDelay, maxDelay, configuration),
        };
    }

    private IEnumerable<ReachableConfig> CreateRange(int minDelay, int maxDelay, Configuration configuration)
    {
        if (minDelay == maxDelay) return 
            [new ReachableConfig(maxDelay, _actionExecutor.Delay(maxDelay, configuration))];
        
        return Enumerable
            .Range(minDelay, (maxDelay-minDelay)+1)
            .Select(i=> new ReachableConfig(i, _actionExecutor.Delay(i, configuration)));
    }

    private ReachableConfig CreateSingle(int i, Configuration configuration)
    {
        return new ReachableConfig(i, _actionExecutor.Delay(i, configuration));
    }


    private (int TimeUntilMinReached, int MaxDelay) PossibleDelaySpan(Configuration configuration)
    {
        var maxDelay = Int32.MaxValue;
        var timeUntilNextMinInvariantReached = maxDelay;
        
        foreach (var (location, locationConfig) in configuration.LocationConfigurations)
            foreach (var (partType, invariant) in location.InvariantsByType)
                foreach (var part in locationConfig.PartsByType[partType])
                {
                    //If any part is at the max we cannot perform any delay
                    if (invariant.Max == part.Age) return (0, 0);
                    
                    maxDelay = Math.Min(maxDelay, invariant.Max - part.Age);
                    timeUntilNextMinInvariantReached = Math.Min(timeUntilNextMinInvariantReached, Math.Max(invariant.Min - part.Age, 0));
                }
        
        return (timeUntilNextMinInvariantReached, maxDelay);
    }
}