using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration;

/// <summary>
/// Used to find all possible ways an action can be executed.
/// </summary>
internal interface IActionExecutionGenerator
{
    
    /// <summary>
    /// Finds all ways the action can be executed under the configuration
    /// </summary>
    /// <param name="action"></param>
    /// <param name="configuration"></param>
    /// <returns>All ways the action can be executed. If it cannot be executed returns []</returns>
    internal IEnumerable<ActionExecution> PossibleWaysToExecute(MoveAction action, Configuration configuration);
}