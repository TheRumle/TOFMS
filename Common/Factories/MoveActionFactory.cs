using System.Collections.ObjectModel;
using Common.JsonTofms.Models;
using Common.Move;

namespace Common.Factories;

public class MoveActionFactory : ITofmsFactory
{
    public IReadOnlyList<MoveAction> CreateMoveActions(TofmSystem tofmSystem)
    {
        LocationStructure[] locationStructures = tofmSystem.Components.SelectMany(e => e.Locations).ToArray();   
        Location[] locations = CreateLocations(locationStructures);
        return CreateMoveAction(tofmSystem.Components.SelectMany(e=>e.Moves), locations);
    }

    private ReadOnlyCollection<MoveAction> CreateMoveAction(IEnumerable<MoveActionStructure> moveActionStructures, Location[] locations)
    {
        var kvs = locations.Select(e => new KeyValuePair<string, Location>(e.Name, e));
        var locationByName = new Dictionary<string, Location>(kvs);

        return moveActionStructures.Select(structure =>
        {
            var emptyBef = structure.EmptyBefore.Select(e => locationByName[e]);
            var emptyAf = structure.EmptyAfter.Select(e => locationByName[e]);
            var parts = structure.Parts.Select(e => new KeyValuePair<string, int>(e.PartType, e.Amount));
            return CreateMoveAction(emptyBef, emptyAf, parts, structure, locationByName);
        }).ToList().AsReadOnly();
    }

    private MoveAction CreateMoveAction(IEnumerable<Location> emptyBef, IEnumerable<Location> emptyAf, IEnumerable<KeyValuePair<string, int>> parts, MoveActionStructure structure, Dictionary<string, Location> locationByName)
    {
        var move = new MoveAction()
        {
            EmptyBefore = new HashSet<Location>(emptyBef),
            EmptyAfter = new HashSet<Location>(emptyAf),
            From = locationByName[structure.From],
            To = locationByName[structure.To],
            PartsToMove = new HashSet<KeyValuePair<string, int>>(parts),
            Name = structure.Name
        };
        return move;
    }
    
    private static Location[] CreateLocations(LocationStructure[] locations)
    {
        return locations.Select(locationStructure =>
            new Location(
                locationStructure.Name,
                locationStructure.Capacity,
                locationStructure.Invariants.Select(e => new Invariant(e.Part, e.Min, e.Max))
            )
        ).ToArray();
    }
}