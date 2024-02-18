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
        var maxPossibleDelay = TimeToNextInvariantLimit(configuration);
        if (maxPossibleDelay <= 0)
        //TODO Figure out how much to delay we can perform 
        

        return _availableActions
            .AsParallel().WithDegreeOfParallelism(4)
            .Select(action => (
                Action: action,
                IsEnabled: _enablednessDecider.IsEnabledUnder(action: action, configuration))
            )
            .Where(action => action.IsEnabled)
            .SelectMany(action => ExecuteAction(action.Action, configuration))
            .Concat(new []{});
    }

    private int TimeToNextInvariantLimit(Configuration configuration)
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
        
        //Generate all possible ways to execute the action (examine all part combinations that enable the action)
        var waysToExecuteAction = ConfigurationExplorer
            .WaysToSatisfyAction(action)
            .Under(locationConfiguration);
        
        //Execute all action in all possible ways
        return waysToExecuteAction
            .Select(wayToExecute => _actionExecutor.Execute(wayToExecute, configuration))
            .Select(reachedConfiguration => new ReachedState(action, reachedConfiguration));
    }
}