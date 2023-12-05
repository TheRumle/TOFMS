using Tofms.Common.JsonTofms.Models;

namespace Common.UnitTests.Factories.fixtures;

public class TofmSystems
{
    private const string partType = "p1";
    private static readonly InvariantDefinition _invariantDefinition = new(partType, 0, 10);

    public TofmJsonSystem SameActionSystem()
    {
        var firstLoc = new LocationDefinition("first", 10, new List<InvariantDefinition>
        {
            _invariantDefinition
        }, false);

        var secondLoc = new LocationDefinition("second", 10, new List<InvariantDefinition>
        {
            _invariantDefinition
        }, false);

        var firstMove = new MoveActionDefinition("firstMove", new List<PartConsumptionDefinition>(),
            firstLoc.Name, secondLoc.Name, new List<string> { firstLoc.Name }, new List<string> { secondLoc.Name });

        var secondMove = new MoveActionDefinition("secondMove", new List<PartConsumptionDefinition>(),
            firstLoc.Name, secondLoc.Name, new List<string> { firstLoc.Name }, new List<string> { secondLoc.Name });

        var component = new TofmComponent
        {
            Locations = new List<LocationDefinition> { firstLoc, secondLoc },
            Name = "Component",
            Moves = new List<MoveActionDefinition>
            {
                firstMove, secondMove
            }
        };


        return new TofmJsonSystem
        {
            Components = new List<TofmComponent> { component },
            Parts = new List<string> { partType }
        };
    }
}