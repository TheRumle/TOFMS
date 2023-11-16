using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Error;

public class UndefinedLocationException : InvalidJsonTofmException
{
    private readonly MoveActionStructure _moveActionStructure;
    private readonly string _locationName;

    public UndefinedLocationException(MoveActionStructure moveActionStructure, string locationName)
    {
        _moveActionStructure = moveActionStructure;
        this._locationName = locationName;
    }
    
    public override string ToString()
    {
        return $"Move action {_moveActionStructure.Name} uses location {_locationName} which has not been defined.";
    }

    public override string ErrorCategory { get; } = "Undefined location";
}