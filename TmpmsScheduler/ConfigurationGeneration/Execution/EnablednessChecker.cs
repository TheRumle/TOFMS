using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration.Execution;

public static class EnablednessChecker
{
    private static bool SatisfiesCapacity(MoveAction action, Configuration configuration) 
        => action.To.Capacity >= configuration.LocationConfigurations[action.To].Size + action.PartsToMove.Sum(e => e.Value);

    private static bool SatisfiesEmptyBefore(MoveAction action, Configuration configuration)
    {
        return configuration.LocationConfigurations
            .Where(e => action.EmptyBefore.Contains(e.Key))
            .All(configs => configs.Value.Size == 0);
    }
    
    private static bool SatisfiesEmptyAfter(MoveAction action, Configuration configuration)
    {
        var allEmptyAfterEmpty = configuration.LocationConfigurations
            .Where(e => action.EmptyAfter.Contains(e.Key) && action.To != e.Key)
            .Select(e => e.Value)
            .All(e => e.Size == 0);

        if (action.EmptyAfter.Contains(action.To))
            return allEmptyAfterEmpty && configuration.RemainingCapacityFor(action.To) >= action.TotalAmountToMove();

        return allEmptyAfterEmpty;
    }


    public static bool IsEnabled(MoveAction action, Configuration under)
    {
        return SatisfiesCapacity(action, under) 
               && SatisfiesEmptyBefore(action, under)
               && SatisfiesEmptyAfter(action, under);
    }
    
    public static bool SatisfiesBaseEnabledness(this MoveAction action, Configuration under)
    {
        return IsEnabled(action, under);
    }
}