
namespace Common.Errors;

public class ValidationViolation
{
    public string Description { get; set; }
}

public class ValidationErrorCollection<T> 
{
    public ValidationErrorCollection(T value)
    {
        this.Value = value;
    }

    private readonly List<ValidationViolation> _violations = new();
    public IReadOnlyList<ValidationViolation> Violations => _violations.AsReadOnly(); 

    public T Value { get; set; }

    public void Add(ValidationViolation violation)
    {
        this._violations.Add(violation);
    }

    public override string ToString()
    {
        var errs = this.Violations.Aggregate("", (s, violation) => s + $"{violation.Description}, ");
        return $@"{Value} of type {typeof(T)} was not valid: {errs}";
    }
}
