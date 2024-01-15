﻿using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Validators;

public interface IValidator<T>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T values);
    Task< IEnumerable<InvalidJsonTofmException> > ValidateAsync(T componentLocations);
}


public interface IValidator<T, TContext>
{
    public IEnumerable<InvalidJsonTofmException> Validate(T locations, TContext moveActions);
    Task< IEnumerable<InvalidJsonTofmException> > ValidateAsync(T componentLocations, TContext context);

}