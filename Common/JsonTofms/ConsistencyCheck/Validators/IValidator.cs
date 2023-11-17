using Common.JsonTofms.ConsistencyCheck.Error;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public interface IValidator<T>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T values);
    public Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(T values);
}


public interface IValidator<T, TContext>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T locations, TContext moveActions);
    public Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(T values, TContext context);
}