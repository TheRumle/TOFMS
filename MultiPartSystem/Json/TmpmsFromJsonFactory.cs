using Tmpms.Journey;
using Tmpms.Json.Convertion;
using Tmpms.Json.Models;
using Tmpms.Move;

namespace Tmpms.Json;

public class TmpmsFromJsonFactory : ITmpmsSystemFactory<TimedMultiPartSystemJsonInput>
{
    public TmpmsFromJsonFactory(IEnumerable<string> parts)
    {
        this._partTypes = parts;
    }

    private readonly IEnumerable<string> _partTypes;

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
        var locations = structure.LocationDeclarations.Select(e => e.ToDomain(_partTypes));
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
                .Select(jsonDefinition => jsonDefinition.ToDomain(_partTypes))
                .ToList();
            journey.Add(partType, locations);
        }

        return journey;
    }
}