using Tmpms;

namespace TMPMSChecker;

public static class ConfigurationExtensions
{

    public static bool IsGoalConfigurationFor(this Configuration configuration, Location goalLocation)
    {
        return !
            configuration.LocationConfigurations
            .Any(e => e.Value.Size > 0 && !e.Key.Equals(goalLocation));
    }
    
}