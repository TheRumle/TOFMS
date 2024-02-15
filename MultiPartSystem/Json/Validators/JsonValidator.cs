using Tmpms.Json.Errors;

namespace Tmpms.Json.Validators;

public abstract class JsonValidator<T> : IValidator<T> 
{
    public IEnumerable<InvalidJsonTmpmsException> Validate(T values)
    {
        var validationTasks = ValidationTasksFor(values);
        return Task.WhenAll(validationTasks).Result.SelectMany(errs=>errs);
    }

    public abstract Task<IEnumerable<InvalidJsonTmpmsException>>[] ValidationTasksFor(T inputs);
}

public abstract class JsonValidator<T, TT> : IValidator<T, TT>
{
    public IEnumerable<InvalidJsonTmpmsException> Validate(T values, TT context)
    {
        var validationTasks = ValidationTasksFor(values, context);
        return Task.WhenAll(validationTasks).Result.SelectMany(e=>e);
    }

    public abstract Task<IEnumerable<InvalidJsonTmpmsException>>[] ValidationTasksFor(T inputs, TT context);
}