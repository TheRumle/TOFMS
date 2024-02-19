using Tmpms;
using Tmpms.Move;
using TmpmsChecker.Algorithm;

namespace TmpmsChecker.ConfigurationGeneration;

internal class AllEnabledActions : IConfigurationGenerator
{
    private readonly IEnumerable<MoveAction> _availableActions;
    private readonly IActionEnablednessDecider _enablednessDecider;
    private readonly IActionExecutor _actionExecutor;

    public AllEnabledActions(
        IEnumerable<MoveAction> availableActions,
        IActionEnablednessDecider enablednessDecider,
        IActionExecutor actionExecutor)
    {
        _availableActions = availableActions;
        _enablednessDecider = enablednessDecider;
        _actionExecutor = actionExecutor;
    }
    
    public IEnumerable<ReachedState> GenerateConfigurations(Configuration configuration)
    {
        return _availableActions
            .AsParallel().WithDegreeOfParallelism(4)
            .Select(action => (
                Action: action,
                IsEnabled: _enablednessDecider.IsEnabledUnder(action: action, configuration))
            )
            .Where(action => action.IsEnabled)
            .SelectMany(action => ExecuteAction(action.Action, configuration))
            .Concat(ComputePossibleDelays(configuration));
    }

    private ParallelQuery<ReachedState> ComputePossibleDelays(Configuration configuration)
    {
        return PossibleDelaySpan(configuration) switch
        {
            (_, <= 0) => Enumerable.Empty<ReachedState>().AsParallel(),
            var (minimumTime, maxPossibleDelay) =>
                Enumerable
                    .Range(minimumTime, minimumTime - maxPossibleDelay)
                    .Select(delay => new ReachedState(delay, _actionExecutor.Delay(delay, configuration))).AsParallel(),
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

    private IEnumerable<ReachedState> ExecuteAction(MoveAction action, Configuration configuration)
    {
        var locationConfiguration = configuration.LocationConfigurations[action.From];
        
        //Generate all possible ways to execute the action (examine all part combinations that enable the action)
        var waysToExecuteAction = ActionSatisfier
            .WaysToSatisfy(action)
            .Under(locationConfiguration);
        
        //Execute all action in all possible ways
        return waysToExecuteAction
            .Select(wayToExecute => _actionExecutor.Execute(wayToExecute, configuration))
            .Select(reachedConfiguration => new ReachedState(action, reachedConfiguration));
    }
}