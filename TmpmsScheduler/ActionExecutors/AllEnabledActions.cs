using Tmpms;
using Tmpms.Move;
using TmpmsChecker.Algorithm;

namespace TmpmsChecker.ActionExecutors;

internal interface IActionExecutor
{
    Configuration Execute(ActionExecution execution, Configuration configuration);
}

internal interface IActionEnablednessDecider
{
    bool IsEnabledUnder(MoveAction action, Configuration configurtion);
}

internal class AllEnabledActions : IConfigurationGenerator
{
    private readonly IEnumerable<MoveAction> _availableActions;
    private readonly IActionEnablednessDecider _enablednessDecider;
    private readonly IActionExecutor _actionExecutor;

    public AllEnabledActions(IEnumerable<MoveAction> availableActions, IActionEnablednessDecider enablednessDecider, IActionExecutor actionExecutor)
    {
        _availableActions = availableActions;
        _enablednessDecider = enablednessDecider;
        _actionExecutor = actionExecutor;
    }
    
    public IEnumerable<ReachedState> GenerateConfigurations(Configuration configuration)
    {
        var maxPossibleDelay = FindMaxDelay(configuration);
        

        return _availableActions
            .AsParallel().WithDegreeOfParallelism(4)
            .Select(action => (
                Action: action,
                IsEnabled: _enablednessDecider.IsEnabledUnder(action: action, configuration))
            )
            .Where(action => action.IsEnabled)
            .SelectMany(action => ExecuteAction(action.Action, configuration));
    }

    private int FindMaxDelay(Configuration configuration)
    {
        var maxDelay = 0;
        foreach (var (location, locationConfig) in configuration.LocationConfigurations)
            foreach (var (partType, invariant) in location.InvariantsByType)
                foreach (var part in locationConfig.PartsByType[partType])
                {
                    //If any part is at the max we cannot perform any delay
                    if (invariant.Max == part.Age) return 0;
                    maxDelay = Math.Min(maxDelay, invariant.Max - part.Age);
                }
        return maxDelay;
    }

    private IEnumerable<ReachedState> ExecuteAction(MoveAction action, Configuration configuration)
    {
        var locationConfiguration = configuration.LocationConfigurations[action.From];
        var waysToExecuteAction = ConfigurationExplorer
            .WaysToSatisfyAction(action)
            .Under(locationConfiguration);

        //For all possible ways to execute the action, execute it and document how to reach the new state
        return waysToExecuteAction
            .Select(wayToExecute => _actionExecutor.Execute(wayToExecute, configuration))
            .Select(reachedConfiguration => new ReachedState(action, reachedConfiguration));
    }
}