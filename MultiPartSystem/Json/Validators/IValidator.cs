﻿using Tmpms.Common.Json.Errors;

namespace Tmpms.Common.Json.Validators;

public interface IValidator<T>
{
    public IEnumerable<InvalidJsonTmpmsException> Validate(T values);
}

public interface IValidator<T, TContext>
{
    public IEnumerable<InvalidJsonTmpmsException> Validate(T values, TContext context);
}