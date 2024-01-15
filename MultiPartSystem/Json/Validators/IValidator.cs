using Tmpms.Common.Json.Errors;

namespace Tmpms.Common.Json.Validators;

public interface IValidator<T>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T values);
    public Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(T inputs);
}

public interface IValidator<T, TContext>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T locations, TContext moveActions);
}