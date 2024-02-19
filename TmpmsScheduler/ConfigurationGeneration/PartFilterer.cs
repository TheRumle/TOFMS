using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration;

public static class PartFilterer
{
    /// <summary>
    /// Finds the part that can be used to execute the given move action.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="parts"></param>
    /// <returns></returns>
    public static Part[] RelevantFor(this IEnumerable<Part> parts, MoveAction action)
    {
        var toLookThrough = parts.ToArray();
        return action.PartsToMove.Keys
            .Select(partType => FindPartsOfType(action, toLookThrough, partType))
            .SelectMany(e => e).ToArray();
    }

    private static Part[] FindPartsOfType(MoveAction action, Part[] partsToLookThrough, string partType)
    {
        var inv = action.From.InvariantsByType[partType];

        Predicate<Part> ageCheck = action.From.IsProcessing
            ? part => part.Age == action.From.InvariantsByType[partType].Max
            : part => part.Age <= inv.Max && part.Age >= inv.Min;
        
        Predicate<Part> check = ageCheck;
        if (action.To.IsProcessing)
            check = part => ageCheck(part) && part.Journey.First() == action.To;
        
        var partsAvailableForMove = partsToLookThrough 
            .Where(part => check.Invoke(part))
            .ToArray();
        
        return partsAvailableForMove;
    }
}