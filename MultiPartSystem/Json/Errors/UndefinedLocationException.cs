using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Errors;

public class UndefinedLocationException : InvalidJsonTmpmsException
{
    private readonly string _locationName;
    private readonly MoveActionDefinition _moveActionDefinition;

    public UndefinedLocationException(MoveActionDefinition moveActionDefinition, string locationName)
    {
        _moveActionDefinition = moveActionDefinition;
        _locationName = locationName;
    }

    public override string ErrorCategory { get; } = "Undefined location";

    public override string ToString()
    {
        return $"Move action {_moveActionDefinition.Name} uses location '{_locationName}' which has not been defined.";
    }
}