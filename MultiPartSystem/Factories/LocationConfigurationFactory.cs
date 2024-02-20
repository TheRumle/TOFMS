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

    public LocationConfiguration ZeroAgedOfPartType(string partType, Location[] journey, int amount = 1)
    {
        var a = new LocationConfiguration(_partTypes);
        for (var i = 0; i < amount; i++)
        {
            a.Add(new Part(partType,0,journey));
        }

        return a;
    }
    
    public LocationConfiguration SingletonOfAge(string partType, Location[] journey, int age)
    {
        var configuration = new LocationConfiguration(_partTypes);
        configuration.Add(new Part(partType,age,journey));
        return configuration;
    }
}