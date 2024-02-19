using System.Diagnostics.Contracts;
using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration.Execution;

internal class ExecutionGenerator : IActionExecutionGenerator
{
    private bool WillExceedCapacity(MoveAction action, Configuration configuration) 
        => action.To.Capacity < configuration.LocationConfigurations[action.To].Size + action.PartsToMove.Sum(e => e.Value);
    
    [Pure]
    public IEnumerable<ActionExecution> PossibleWaysToExecute(MoveAction action, Configuration configuration)
    {
        if (WillExceedCapacity(action, configuration)) return [];
        
        return PossibleMoves(action, configuration.LocationConfigurations[action.From])
            .Values
            .CartesianProduct()
            .Select(e=>new ActionExecution(e));
    }
        
    private static Dictionary<string, IEnumerable<ConsumeProduceSet>> PossibleMoves(MoveAction action, LocationConfiguration locConfFrom)
    {
        Func<IEnumerable<Part>, ConsumeProduceSet> factory = action.To.IsProcessing ? ConsumeProduceSet.ConstructWithJourneyUpdate : ConsumeProduceSet.Construct;
        Dictionary<string, IEnumerable<ConsumeProduceSet>> possibleThingsToConsumeProduce = new();
        foreach (var (partType, parts) in locConfFrom.PartsByType)
        {
            IEnumerable<ConsumeProduceSet> enumerable = Combiner
                .AllCombinationsOfSize(FindSatisfyingParts(action, partType, parts), action.PartsToMove[partType])
                .Select(factory.Invoke);

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
        //Is sufficient age
        Predicate<Part> ageOkay = part => part.Age <= inv.Max || part.Age >= inv.Min;
        if (action.From.IsProcessing)
        {
            ageOkay = part => part.Age == action.From.InvariantsByType[partType].Max;
        }

        Predicate<Part> predicate = ageOkay;
        if (action.To.IsProcessing) predicate = part => ageOkay(part) && part.Journey.First() == action.To;
            
        var partsAvailableForMove = parts 
            .Where(part => predicate.Invoke(part))
            .ToArray();
        
        return partsAvailableForMove;
    }
}