using Common.JsonTofms.Models;

namespace Common.UnitTests.Factories.fixtures;

public class TofmSystems
{
    private const string partType = "p1";
    private static InvariantStructure _invariantStructure = new InvariantStructure(partType, 0, 10);

    public TofmSystem SameActionSystem()
    {
        var firstLoc = new LocationStructure("first", 10, new List<InvariantStructure>()
        {
            _invariantStructure
        });
        
        var secondLoc = new LocationStructure("second", 10, new List<InvariantStructure>()
        {
            _invariantStructure
        });
        
        var firstMove = new MoveActionStructure("firstMove",new List<(int Amount, string PartType)>(),
            firstLoc.Name, secondLoc.Name, new List<string>(){firstLoc.Name}, new List<string>(){secondLoc.Name}  );
        
        var secondMove = new MoveActionStructure("secondMove",new List<(int Amount, string PartType)>(),
            firstLoc.Name, secondLoc.Name, new List<string>(){firstLoc.Name}, new List<string>(){secondLoc.Name}  );

        var component = new TofmComponent()
        {
            Locations = new List<LocationStructure>() { firstLoc, secondLoc },
            Name = "Component",
            Moves = new List<MoveActionStructure>()
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