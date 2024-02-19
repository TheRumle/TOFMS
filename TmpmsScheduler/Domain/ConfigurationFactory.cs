using Tmpms;

namespace TmpmsChecker.Domain;

public class ConfigurationFactory
{

    public Configuration Singleton(Location location, LocationConfiguration conf)
    {
        return new Configuration(new []{KeyValuePair.Create(location, conf)});
    }
    
    public Configuration From(IEnumerable<(Location location, LocationConfiguration configuration)> locConfs)
    {
        return new Configuration(locConfs.Select(e=> KeyValuePair.Create(e.location, e.configuration)));
    }
    
    
}