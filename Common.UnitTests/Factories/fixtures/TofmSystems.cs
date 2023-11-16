using Common.JsonTofms.Models;

namespace Common.UnitTests.Factories.fixtures;

public class TofmSystems
{
    private const string partType = "p1";
    private static InvariantDefinition _invariantDefinition = new InvariantDefinition(partType, 0, 10);

    public TofmSystem SameActionSystem()
    {
        var firstLoc = new LocationDefinition("first", 10, new List<InvariantDefinition>()
        {
            _invariantDefinition
        });
        
        var secondLoc = new LocationDefinition("second", 10, new List<InvariantDefinition>()
        {
            _invariantDefinition
        });
        
        var firstMove = new MoveActionDefinition("firstMove",new List<(int Amount, string PartType)>(),
            firstLoc.Name, secondLoc.Name, new List<string>(){firstLoc.Name}, new List<string>(){secondLoc.Name}  );
        
        var secondMove = new MoveActionDefinition("secondMove",new List<(int Amount, string PartType)>(),
            firstLoc.Name, secondLoc.Name, new List<string>(){firstLoc.Name}, new List<string>(){secondLoc.Name}  );

        var component = new TofmComponent()
        {
            Locations = new List<LocationDefinition>() { firstLoc, secondLoc },
            Name = "Component",
            Moves = new List<MoveActionDefinition>()
            {
                firstMove, secondMove
            }
        };


        return new TofmSystem()
        {
            Components = new List<TofmComponent>() { component },
            Parts = new List<string>() { partType }
        };
    }
}