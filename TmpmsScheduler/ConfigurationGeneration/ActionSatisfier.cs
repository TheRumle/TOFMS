using System.Diagnostics.Contracts;
using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration;

internal static class ActionSatisfier
{
    
    [Pure]
    public static SatisfyAction WaysToSatisfy(MoveAction action)
    {
        return new SatisfyAction(action);
    }
        
    internal class SatisfyAction(MoveAction action)
    {
        [Pure]
        public IEnumerable<ActionExecution> Under(LocationConfiguration locConfFrom)
        {
            return PossibleMoves(action, locConfFrom)
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
            if (action.To.IsProcessing)
            {
                predicate = part => ageOkay(part) && part.Journey.First() == action.To;
            }
            var partsAvailableForMove = parts 
                .Where(part => predicate.Invoke(part))
                .ToArray();
            return partsAvailableForMove;
        }
    }
}