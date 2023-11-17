using System.Text;
using Common.JsonTofms.ConsistencyCheck.Error;

namespace Common.JsonTofms.ConsistencyCheck;

public class ErrorFormatter
{
    public ErrorFormatter(InvalidJsonTofmException[] errors)
    {
        Errors = errors;
    }

    public IEnumerable<InvalidJsonTofmException> Errors { get; init; }

    public string ToErrorString()
    {
        var bob = new StringBuilder("Found errors:\n");
        var errorsByLocationName = Errors.GroupBy(e => e.ErrorCategory);

            bob.AppendLine("\n \n ___-___-PARSE ERRORS START-___-___\n\n");
        foreach (var errGroup in errorsByLocationName)
        {
            bob.AppendLine($"'{errGroup.Key} errors:'").Append('\n');
            foreach (var exception in errGroup) bob.Append('\t').Append(exception).Append("\n\n");
        }

            bob.AppendLine("\n\n--___-___-PARSE ERRORS END-___-___-- \n \n");
        return bob.ToString();
    }
}