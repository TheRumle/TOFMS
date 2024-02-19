using Tmpms;

namespace TmpmsChecker.ConfigurationGeneration;

internal interface IActionExecutor
{
    Configuration Execute(ActionExecution execution, Configuration configuration);
    Configuration Delay(int delay, Configuration configuration);
}