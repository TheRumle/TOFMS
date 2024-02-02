using Tmpms.Common.Json.Models;
using Tmpms.Common.Move;

namespace Tmpms.Common.Json.Convertion;

public static class JsonToDomain
{
    public static Location ToDomain(this LocationDefinition locationDefinition)
    {
        var invariants = locationDefinition.Invariants.Select(e => new Invariant(e.Part, e.Min, e.Max));
        return new Location(locationDefinition.Name, locationDefinition.Capacity, invariants,
            locationDefinition.IsProcessing);
    }

    public static MoveAction ToDomain(this MoveActionDefinition actionDefinition, IEnumerable<Location> locations)
    {
        var enumerable = locations as Location[] ?? locations.ToArray();
        return new MoveAction
        {
            From = enumerable.First(e => e.Name == actionDefinition.From),
            Name = actionDefinition.Name,
            To = enumerable.First(e => e.Name == actionDefinition.To),
            EmptyAfter = enumerable.Where(e => actionDefinition.EmptyAfter.Contains(e.Name)).ToHashSet(),
            EmptyBefore = enumerable.Where(e => actionDefinition.EmptyBefore.Contains(e.Name)).ToHashSet(),
            PartsToMove = actionDefinition.Parts.ToHashSet(),
        };
    }
}