using System.Text;
using Tmpms.Common.Json.Errors;

namespace Tmpms.Common.Json;

public class ErrorFormatter
{

    public ErrorFormatter(InvalidJsonTofmException[] errors)
    {
        Errors = errors;
    }

    public IEnumerable<InvalidJsonTofmException> Errors { get; init; }

    public string ToErrorString()
    {
        var bob = new StringBuilder($"Found errors in TMPMS json:\n");
        foreach (var errGroup in  Errors.GroupBy(e => e.ErrorCategory))
        {
            bob.AppendLine($"'{errGroup.Key} errors:'").Append('\n');
            foreach (var exception in errGroup) bob.Append('\t').Append(exception).Append("\n\n");
        }

        return bob.ToString();
    }
}