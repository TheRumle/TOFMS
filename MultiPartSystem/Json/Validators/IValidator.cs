﻿using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

namespace Tmpms.Common.Json.Validators;

public interface IValidator<T>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T values);
    public Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasks(T inputs);
}

public interface IValidator<T, TContext>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T locations, TContext moveActions);
}