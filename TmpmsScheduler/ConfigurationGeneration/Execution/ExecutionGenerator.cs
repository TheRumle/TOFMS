using System.Diagnostics.Contracts;
using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration.Execution;

public static class PartFilterer
{
    

    /// <summary>
    /// Finds the part that can be used to execute the given move action.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="parts"></param>
    /// <returns></returns>
    public static Part[] FindPartsRelevantFor(MoveAction action, IEnumerable<Part> parts)
    {
        var toLookThrough = parts.ToArray();
        return action.PartsToMove.Keys
            .Select(partType => FindPartsOfType(action, toLookThrough, partType))
            .SelectMany(e => e).ToArray();
    }

    private static Part[] FindPartsOfType(MoveAction action, Part[] partsToLookThrough, string partType)
    {
        var inv = action.From.InvariantsByType[partType];
        Predicate<Part> predicate = action.From.IsProcessing
            ? part => part.Age == action.From.InvariantsByType[partType].Max  && part.Journey.First() == action.To
            : part => part.Age <= inv.Max || part.Age >= inv.Min;
            
                
        var partsAvailableForMove = partsToLookThrough 
            .Where(part => predicate.Invoke(part))
            .ToArray();
        
        return partsAvailableForMove;
    }
}

internal class ExecutionGenerator : IActionExecutionGenerator
{
    [Pure]
    public IEnumerable<ActionExecution> PossibleWaysToExecute(MoveAction action, Configuration under)
    {
        if (!EnablednessChecker.IsEnabled(action,under)) return [];
        
        return PossibleMoves(action, under.LocationConfigurations[action.From])
            .CartesianProduct()
            .Select(e=>new ActionExecution(e));
    }

        
    private static List<IEnumerable<ConsumeProduceSet>> PossibleMoves(MoveAction action, LocationConfiguration locConfFrom)
    {
       List<IEnumerable<ConsumeProduceSet>> possibleThingsToConsumeProduce = new();

        Dictionary<string, Part[]> relevantParts = PartFilterer.FindPartsRelevantFor(action, locConfFrom.AllParts)
            .GroupBy(part => part.PartType, x => x)
            .ToDictionary(g => g.Key, g => g.ToArray());
        
        foreach (var (partType, amount) in action.PartsToMove)
        {
            IEnumerable<ConsumeProduceSet> enumerable = Combiner
                .AllCombinationsOfSize(relevantParts[partType], amount)
                .Select(val => action.To.IsProcessing
                    ? ConsumeProduceSet.ConstructWithJourneyUpdate(val)
                    : ConsumeProduceSet.Construct(val));
            
            if (!enumerable.Any()) return [];
            possibleThingsToConsumeProduce.Add(enumerable);
        }

        return possibleThingsToConsumeProduce;
    }
}