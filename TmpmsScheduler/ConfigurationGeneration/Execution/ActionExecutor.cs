using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration.Execution;

internal class ActionExecutor : IActionExecutor
{
    public Configuration Execute(Location from, Location to, ActionExecution execution, Configuration configuration)
    {
        var copy = configuration.Copy();
        var (consume, produce) = execution.ToPartDictionary();
        
        copy.LocationConfigurations[from].Remove(consume);
        copy.LocationConfigurations[to].Add(produce);
        
        return copy;
    }

    public Configuration Delay(int delay, Configuration configuration)
    {
        var copy = configuration.Copy();
        var allParts = copy.LocationConfigurations.Values.SelectMany(e => e.AllParts);
        foreach (var part in allParts)
            part.Age += delay;

        return copy;
    }
}