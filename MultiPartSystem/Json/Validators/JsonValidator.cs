﻿using Tmpms.Common.Json.Errors;

namespace Tmpms.Common.Json.Validators;

public abstract class JsonValidator<T> : IValidator<T> 
{
    public IEnumerable<InvalidJsonTofmException> Validate(T values)
    {
        var validationTasks = ValidationTasksFor(values);
        return Task.WhenAll(validationTasks).Result.SelectMany(e=>e);
    }

    public abstract Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(T inputs);
}

public abstract class JsonValidator<T, TT> : IValidator<T, TT> 
{
    public IEnumerable<InvalidJsonTofmException> Validate(T values, TT context)
    {
        var validationTasks = ValidationTasksFor(values, context);
        return Task.WhenAll(validationTasks).Result.SelectMany(e=>e);
    }

    public abstract Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(T inputs, TT context);
}