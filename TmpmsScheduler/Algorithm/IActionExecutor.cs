using Tmpms;

namespace TmpmsChecker.Algorithm;

public interface IActionExecutor
{
    public IEnumerable<Configuration> ExecuteActions(Configuration configuration);
}