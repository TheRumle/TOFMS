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
            .CartesianProduct()
            .Select(e=>new ActionExecution(e));
    }

        
    private static List<IEnumerable<ConsumeProduceSet>> PossibleMoves(MoveAction action, LocationConfiguration locConfFrom)
    {

        var relevantParts = locConfFrom
            .AllParts
            .RelevantFor(action)
            .GroupBy(part => part.PartType, x => x)
            .ToDictionary(g => g.Key, g => g.ToArray());
        
        List<IEnumerable<ConsumeProduceSet>> possibleThingsToConsumeProduce = new();
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