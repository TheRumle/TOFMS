using Tmpms;
using Tmpms.Move;
using TmpmsChecker.Algorithm;

namespace TmpmsChecker.ActionExecutors;

public record PossibleMove(IEnumerable<Part> Consume, IEnumerable<Part> Produce)
{
    public static PossibleMove WithJourneyUpdate(IEnumerable<Part> consume)
    {
        Part[] enumerable = consume as Part[] ?? consume.ToArray();
        return new PossibleMove(enumerable,enumerable.Select(e => new Part(e.PartType, 0, e.Journey.Skip(1))));
    }
    
    public static PossibleMove FromConsumed(IEnumerable<Part> consume)
    {
        Part[] enumerable = consume as Part[] ?? consume.ToArray();
        return new PossibleMove(enumerable,enumerable.Select(e => e with { Age = 0 }));
    }
    
}


public class AllEnabledActions : IConfigurationGenerator
{
    private readonly IEnumerable<MoveAction> _availableActions;
    private readonly Func<MoveAction, Configuration, Boolean> IsEnabledUnder;
    private readonly string[] _allPartTypes;

    public AllEnabledActions(IEnumerable<MoveAction> availableActions, string[] partTypes, Func<MoveAction, Configuration, Boolean> isEnabledUnder)
    {
        _availableActions = availableActions;
        IsEnabledUnder = isEnabledUnder;
        _allPartTypes = partTypes;
    }
    
    public IEnumerable<ReachedState> GenerateConfigurations(Configuration configuration)
    {
        var actions = _availableActions.Where(a => IsEnabledUnder(a, configuration));
        foreach (var ac in actions)
        {
            var result = ExecuteAction(ac, configuration);
            if (result == null) continue;
            yield return result;
        }
    }

    private ReachedState? ExecuteAction(MoveAction action, Configuration configuration)
    {
        var copy = configuration.Copy();
        var total = action.TotalAmountToMove();
        
        var locConfTo = configuration.LocationConfigurations[action.To];
        var locConfFrom = configuration.LocationConfigurations[action.From];
        if (locConfTo.Size + action.TotalAmountToMove() > action.To.Capacity)
            return null;
        
        var consumptionPossibilities = CreatePossibleConsumptions(action, locConfFrom);
        if (consumptionPossibilities == null) return null;
        var productionpossibilities = CreatePossibleProductions(action, locConfTo, consumptionPossibilities);
        
        
        
        

        
    
        
        
        
        throw new NotImplementedException();
    }

    private IEnumerable<PossibleMove> CreatePossibleProductions(MoveAction action, LocationConfiguration locConfTo,
        Dictionary<string, IEnumerable<IEnumerable<Part>>> possibleConsumptions)
    {
        return action.To.IsProcessing ? CreateWithUpdatedJourneys(possibleConsumptions) : CreateWithOriginalJourneys(possibleConsumptions);
    }

    private IEnumerable<PossibleMove> CreateWithOriginalJourneys( Dictionary<string, IEnumerable<IEnumerable<Part>>> possibleConsumptions)
    {

    }

    private IEnumerable<PossibleMove> CreateWithUpdatedJourneys( Dictionary<string, IEnumerable<IEnumerable<Part>>> possibleConsumptions)
    {
        return possibleConsumptions.Select(kvp => KeyValuePair.Create(kvp.Key, kvp.Value
            .Select(parts => parts.Select(e => new Part(e.PartType, 0, e.Journey.Skip(1))))));
    }

    private static Dictionary<string, IEnumerable<IEnumerable<Part>>>? CreatePossibleConsumptions(MoveAction action, LocationConfiguration locConfFrom)
    {
        Dictionary<string, IEnumerable<IEnumerable<Part>>> combinationPerType = new();
        foreach (var (partType, parts) in locConfFrom.PartsByType)
        {
            var amountToMove = action.PartsToMove[partType];
            var partsAvailableForMove = FindSatisfyingParts(action, partType, parts);
            if (partsAvailableForMove.Length < amountToMove) return null;
            var partCombinations = Combiner.CombinationsOfSize(partsAvailableForMove, amountToMove);
            combinationPerType[partType] = partCombinations;
        }

        return combinationPerType;
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