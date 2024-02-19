using System.Diagnostics.Contracts;
using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration.Execution;

internal class ExecutionGenerator : IActionExecutionGenerator
{
    [Pure]
    public IEnumerable<ActionExecution> PossibleWaysToExecute(MoveAction action, Configuration under)
    {
        if (!EnablednessChecker.IsEnabled(action,under)) return [];
        
        return PossibleMoves(action, under.LocationConfigurations[action.From])
            .Values
            .CartesianProduct()
            .Select(e=>new ActionExecution(e));
    }

        
    private static Dictionary<string, IEnumerable<ConsumeProduceSet>> PossibleMoves(MoveAction action, LocationConfiguration locConfFrom)
    {
        Dictionary<string, IEnumerable<ConsumeProduceSet>> possibleThingsToConsumeProduce = new();
        foreach (var (partType, parts) in locConfFrom.PartsByType)
        {

            IEnumerable<ConsumeProduceSet> enumerable = Combiner
                .AllCombinationsOfSize(FindSatisfyingParts(action, partType, parts), action.PartsToMove[partType])
                .Select(parts => action.To.IsProcessing
                    ? ConsumeProduceSet.ConstructWithJourneyUpdate(parts)
                    : ConsumeProduceSet.Construct(parts));
            
            if (!enumerable.Any()) return [];
            possibleThingsToConsumeProduce[partType] = enumerable;
        }

        return possibleThingsToConsumeProduce;
    }

    /// <summary>
    ///  Finds the parts of the given part type which are old enough to be considered for the move.
    ///  If the given moveaction's To component is a processing-location, then it reduces the journey.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="partType"></param>
    /// <param name="parts"></param>
    /// <returns></returns>
    private static Part[] FindSatisfyingParts(MoveAction action, string partType, List<Part> parts)
    {
        var inv = action.From.InvariantsByType[partType];
        Predicate<Part> predicate = action.From.IsProcessing
            ? part => part.Age == action.From.InvariantsByType[partType].Max  && part.Journey.First() == action.To
            : part => part.Age <= inv.Max || part.Age >= inv.Min;
        
            
        var partsAvailableForMove = parts 
            .Where(part => predicate.Invoke(part))
            .ToArray();
        
        return partsAvailableForMove;
    }
}