using Tmpms;
using Tmpms.Move;
using TmpmsChecker.Algorithm;

namespace TmpmsChecker.ActionExecutors;

public class AllEnabledActions : IConfigurationGenerator
{
    private readonly IEnumerable<MoveAction> _availableActions;
    private readonly Func<MoveAction, Configuration, Boolean> IsEnabledUnder;

    public AllEnabledActions(IEnumerable<MoveAction> availableActions, Func<MoveAction, Configuration, Boolean> isEnabledUnder)
    {
        _availableActions = availableActions;
        IsEnabledUnder = isEnabledUnder;
    }
    
    public IEnumerable<ReachedState> GenerateConfigurations(Configuration configuration)
    {
        var actions = _availableActions.Where(a => IsEnabledUnder(a, configuration));
        return actions.Select(action => ExecuteAction(action, configuration));
    }

    private ReachedState ExecuteAction(MoveAction action, Configuration configuration)
    {
        var copy = configuration.Copy();
        
        var total = action.TotalAmountToMove();
        List<Part> partsToRemove = new List<Part>(total);
        foreach (var (part, amount) in action.PartsToMove)
        {
            var toMoveFrom = configuration.LocationConfigurations[action.From];
            
            
            
        }
        
        
        
        
        
        throw new NotImplementedException();
    }
}