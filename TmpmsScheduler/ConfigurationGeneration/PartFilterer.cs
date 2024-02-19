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
        Predicate<Part> predicate = action.From.IsProcessing
            ? part => part.Age == action.From.InvariantsByType[partType].Max  && part.Journey.First() == action.To
            : part => part.Age <= inv.Max || part.Age >= inv.Min;
            
                
        var partsAvailableForMove = partsToLookThrough 
            .Where(part => predicate.Invoke(part))
            .ToArray();
        
        return partsAvailableForMove;
    }
}