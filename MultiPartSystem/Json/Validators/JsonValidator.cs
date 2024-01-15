using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

namespace Tmpms.Common.Json.Validators;

public abstract class JsonValidator<T> : Validators.IValidator<T> 
{
    public IEnumerable<InvalidJsonTofmException> Validate(T values)
    {
        var validationTasks = ValidationTasksFor(values);
        return Task.WhenAll(validationTasks).Result.SelectMany(e=>e);
    }

    public abstract Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(T inputs);
}