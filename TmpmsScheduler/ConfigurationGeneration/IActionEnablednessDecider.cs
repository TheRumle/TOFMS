using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker.ConfigurationGeneration;

internal interface IActionEnablednessDecider
{
    bool IsEnabledUnder(MoveAction action, Configuration configuration);
}