using System.Text;
using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

namespace Tmpms.Common.JsonTofms.ConsistencyCheck;

public class ErrorFormatter
{

    public ErrorFormatter(InvalidJsonTofmException[] errors)
    {
        Errors = errors;
    }

    public IEnumerable<InvalidJsonTofmException> Errors { get; init; }

    public string ToErrorString()
    {
        var bob = new StringBuilder($"Found errors in component:\n");
        var errorsByLocationName = Errors.GroupBy(e => e.ErrorCategory);

        foreach (var errGroup in errorsByLocationName)
        {
            bob.AppendLine($"'{errGroup.Key} errors:'").Append('\n');
            foreach (var exception in errGroup) bob.Append('\t').Append(exception).Append("\n\n");
        }

        return bob.ToString();
    }
}