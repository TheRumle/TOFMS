using Tmpms.Json.Errors;

namespace Tmpms.Json.Validators;

public interface IValidator<T>
{
    public IEnumerable<InvalidJsonTmpmsException> Validate(T values);
}

public interface IValidator<T, TContext>
{
    public IEnumerable<InvalidJsonTmpmsException> Validate(T values, TContext context);
}