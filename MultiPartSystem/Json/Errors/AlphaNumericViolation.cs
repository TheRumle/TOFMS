﻿namespace Tmpms.Common.Json.Errors;

public class AlphaNumericViolation<TContext> : InvalidJsonTmpmsException
{
    private readonly TContext context;
    private readonly string violation;

    public AlphaNumericViolation(TContext context, string violation)
    {
        this.context = context;
        this.violation = violation;
    }
    public override string ErrorCategory { get; } = "Invalid Name";
    public override string ToString()
    {
        return $"\"{violation}\" is not an alphanumeric name! It was found in {context.GetType().Name}:\n\t {context}";
    }
}