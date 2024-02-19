using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration.Execution;

public class EnablednessDecider : IActionEnablednessDecider
{
    public bool IsEnabledUnder(MoveAction action, Configuration configuration)
    {
        return !WillExceedCapacity(action, configuration);
    }

    private bool WillExceedCapacity(MoveAction action, Configuration configuration) 
        => action.To.Capacity < configuration.LocationConfigurations[action.To].Size + action.PartsToMove.Sum(e => e.Value);
}