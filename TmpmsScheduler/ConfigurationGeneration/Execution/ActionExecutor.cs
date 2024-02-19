using Tmpms;

namespace TmpmsChecker.ConfigurationGeneration.Execution;

internal class ActionExecutor : IActionExecutor
{
    public Configuration Execute(Location from, Location to, ActionExecution execution, Configuration configuration)
    {
        var copy = configuration.Copy();
        copy.LocationConfigurations[from].Remove(execution.Moves.SelectMany(e=>e.Consume));
        copy.LocationConfigurations[to].Add(execution.Moves.SelectMany(e=>e.Produce));
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