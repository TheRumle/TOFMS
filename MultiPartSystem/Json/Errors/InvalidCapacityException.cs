using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Errors;

public class InvalidCapacityException : InvalidJsonTofmException
{
    private readonly LocationDefinition _def;

    public InvalidCapacityException(LocationDefinition locationDefinition)
    {
        _def = locationDefinition;
    }

    public override string ErrorCategory { get; } = "InvalidCapacityException";
    public override string ToString()
    {
        return $"The definition of {_def.Name} has invalid capacity of {_def.Capacity}";
    }
}