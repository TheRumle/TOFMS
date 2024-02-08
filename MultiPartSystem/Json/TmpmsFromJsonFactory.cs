using Tmpms.Common.Journey;
using Tmpms.Common.Json.Convertion;
using Tmpms.Common.Json.Models;
using Tmpms.Common.Move;

namespace Tmpms.Common.Json;

public class TmpmsFromJsonFactory : ITmpmsSystemFactory<TimedMultiPartSystemJsonInput>
{
    public TimedMultipartSystem Create(TimedMultiPartSystemJsonInput structure)
    {
        var moveActions = CreateMoveActions(structure).ToArray();
        return new TimedMultipartSystem()
        {
            Journeys = CreateJourneys(structure),
            MoveActions = moveActions,
            Parts = structure.Parts,
            Locations = moveActions.SelectMany(e => e.InvolvedLocations).ToHashSet()
        };
    }

    private IEnumerable<MoveAction> CreateMoveActions(TimedMultiPartSystemJsonInput structure)
    {
        var locations = structure.LocationDeclarations.Select(e => e.ToDomain());
        return structure.Actions.Select(e => e.ToDomain(locations));
    }

    private JourneyCollection CreateJourneys(TimedMultiPartSystemJsonInput structure)
    {
        JourneyCollection journey = new();
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