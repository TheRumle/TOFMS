namespace Tmpms.Common.Json.Errors;

public class DuplicateDefinitionError<T> : InvalidJsonTmpmsException
{
    private readonly IEnumerable<T> _values;

    public DuplicateDefinitionError(IEnumerable<T> values)
    {
        _values = values;
    }

    public override string ErrorCategory { get; } = $"Duplicate {typeof(T).Name} Declaration";
    public override string ToString()
    {
        var valuesString = _values.Select(e => e.ToString()).Aggregate("",(prev, curr)=>prev + $", {curr}");
        return $"The defined TMPMS had duplicate {typeof(T).Name} declarations: '{valuesString}";
    }
}