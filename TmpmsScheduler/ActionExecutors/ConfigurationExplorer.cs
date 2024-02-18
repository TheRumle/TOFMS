using System.Diagnostics.Contracts;
using Tmpms.Move;

namespace TmpmsChecker.ActionExecutors;

internal static class ConfigurationExplorer
{
    [Pure]
    public static SatisfiableConfigurationsBuild WaysToSatisfyAction(MoveAction action)
    {
        return new SatisfiableConfigurationsBuild(action);
    }
}