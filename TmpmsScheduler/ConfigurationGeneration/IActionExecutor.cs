using Tmpms;

namespace TmpmsChecker.ConfigurationGeneration;

internal interface IActionExecutor
{
    Configuration Execute(Location from, Location to, ActionExecution execution, Configuration configuration);
    Configuration Delay(int delay, Configuration configuration);
}