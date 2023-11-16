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
        
        foreach (var errGroup in errorsByLocationName)
        {
            bob.AppendLine("Errors for").Append(errGroup.Key).Append(": ").Append(errGroup);
            foreach (var exception in errGroup) bob.Append('\t').Append(exception);
        }

        return bob.ToString();
    }
}