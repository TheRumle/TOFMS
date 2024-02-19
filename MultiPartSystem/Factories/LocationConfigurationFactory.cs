namespace Tmpms.Factories;

public class LocationConfigurationFactory
{
    private readonly IEnumerable<string> _partTypes;

    public LocationConfigurationFactory(IEnumerable<string> partTypes)
    {
        _partTypes = partTypes;
    }
    
    public LocationConfiguration Empty()
    {
        return new LocationConfiguration(_partTypes);
    }

    public LocationConfiguration ZeroAgedOfPart(string partType, Location[] journey, int amount = 1)
    {
        var a = new LocationConfiguration(_partTypes);
        for (var i = 0; i < amount; i++)
        {
            a.Add(new Part(partType,0,journey));
        }

        return a;
    }
}