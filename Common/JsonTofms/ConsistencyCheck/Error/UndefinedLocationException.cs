using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Error;

public class UndefinedLocationException : InvalidJsonTofmException
{
    private readonly MoveActionDefinition _moveActionDefinition;
    private readonly string _locationName;

    public UndefinedLocationException(MoveActionDefinition moveActionDefinition, string locationName)
    {
        _moveActionDefinition = moveActionDefinition;
        this._locationName = locationName;
    }
    
    public override string ToString()
    {
        return $"Move action {_moveActionDefinition.Name} uses location '{_locationName}' which has not been defined.";
    }

    public override string ErrorCategory { get; } = "Undefined location";
}