using System.Collections.Generic;
using System.Linq;
using TestDataGenerator;
using Tmpms;
using Tmpms.Factories;
using Tmpms.Move;
using TmpmsChecker.Algorithm;

namespace TmpmsScheduler.UnitTest;

public class SinglePartMoveTest
{
      
    public (Location from, Location to, Configuration configuration) PrepareConfiguration(string part, int parts, int capacity)
    {
        LocationGenerator generator = new LocationGenerator(new List<string> { part });
        Location from = generator.GetRegular();
        Location to = generator.GetRegular() with { Capacity = capacity };
        
        ConfigurationFactory configurationFactory = new ConfigurationFactory();
        LocationConfigurationFactory locConfFactory = new LocationConfigurationFactory([part]);
        
        var configuration = configurationFactory.From(new List<(Location, LocationConfiguration)>
        {
            (from, CreateLocationConfigurationWithUniqueLengthJourney(part, parts, generator)),
            (to, locConfFactory.Empty())
        });

        return (from, to, configuration);
    }

    public MoveAction CreateMoveAction(string part, int parts, Location from, Location to)
    {
        return CreateMoveAction(part, parts, from, to, [], []);
    }
    
    public MoveAction CreateMoveAction(string part, int parts, Location from, Location to, HashSet<Location> emptyAfter, HashSet<Location> emptyBefore)
    {
        return new MoveAction()
        {
            Name = "Test",
            From = from,
            To = to,
            PartsToMove = new Dictionary<string, int> { { part, parts } },
            EmptyBefore = emptyBefore,
            EmptyAfter = emptyAfter
        };
    }


    public LocationConfiguration CreateLocationConfigurationWithUniqueLengthJourney(string partType, int amount, LocationGenerator generator)
    {
        var jour = Enumerable.Repeat(generator.GetProcessing(), amount).ToArray();
        var a = new LocationConfiguration([partType]);
        for (var i = 0; i < amount; i++)
            a.Add(new Part(partType,0,jour.Skip(1)));

        return a;
    }
}