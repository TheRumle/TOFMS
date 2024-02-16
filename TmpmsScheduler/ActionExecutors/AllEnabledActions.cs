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
        foreach (var ac in actions)
        {
            var result = ExecuteAction(ac, configuration);
            if (result == null) continue;
            yield return result;
        }
    }

    private ReachedState ExecuteAction(MoveAction action, Configuration configuration)
    {
        var copy = configuration.Copy();
        var total = action.TotalAmountToMove();
        List<Part> partsToRemove = new List<Part>(total);
        foreach (var (partType, amount) in action.PartsToMove)
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
                predicate = (Part part) => ageOkay(part) && part.Journey.Peek() == action.To;
            }


            var availableToMove = configuration.LocationConfigurations[action.From]
                .PartsByType[partType]
                .Where(part => predicate.Invoke(part));
            
            if (availableToMove.Count() < action.PartsToMove)
        }
        
        
        
        throw new NotImplementedException();
    }
}