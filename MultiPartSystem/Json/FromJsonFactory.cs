using Tmpms.Common.Json.Convertion;
using Tmpms.Common.Move;

namespace Tmpms.Common.Json;

internal class FromJsonFactory : ITmpmsSystemFactory<TimedMultiPartSystemJsonInput>
{
    public TimedMultipartSystem Create(TimedMultiPartSystemJsonInput structure)
    {
        return new TimedMultipartSystem()
        {
            Journeys = CreateJourneys(structure),
            MoveActions = CreateMoveActions(structure),
            Parts = structure.Parts
        };
    }

    private IEnumerable<MoveAction> CreateMoveActions(TimedMultiPartSystemJsonInput structure)
    {
        var locations = structure.LocationDeclarations.Select(e => e.ToDomain());
        return structure.Actions.Select(e => e.ToDomain(locations));
    }

    private Dictionary<string, IEnumerable<Location>> CreateJourneys(TimedMultiPartSystemJsonInput structure)
    {

        Dictionary<string, IEnumerable<Location>> journey = new();
        foreach (var kvp in structure.Journeys)
        {
            var partType = kvp.Key;
            var locationNames = kvp.Value;
            
            var locations = structure
                .LocationDeclarations
                .Where(e => locationNames.Contains(e.Name))
                .Select(jsonDefinition => jsonDefinition.ToDomain())
                .ToList();
            journey.Add(partType, locations);
        }

        return journey;
    }
}