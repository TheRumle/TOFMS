using Tmpms;

namespace TmpmsChecker.Algorithm;

public interface IActionExecutor
{
    public IEnumerable<ReachedState> ExecuteAction(Configuration configuration);
}