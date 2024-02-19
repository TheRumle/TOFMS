using Tmpms;

namespace TmpmsChecker.Domain;

public class LocationConfigurationFactory
{
    private readonly IEnumerable<string> _partTypes;
    private Random _random = new();

    public LocationConfigurationFactory(IEnumerable<string> partTypes)
    {
        _partTypes = partTypes;
    }
    
    public LocationConfiguration Empty()
    {
        return new LocationConfiguration(_partTypes);
    }

    public LocationConfiguration ZeroAgedOfPartType(string partType, Tmpms.Location[] journey, int amount = 1)
    {
        var a = new LocationConfiguration(_partTypes);
        for (var i = 0; i < amount; i++)
            a.Add(new Part(_random.Next(), partType,0,journey));

        return a;
    }
}