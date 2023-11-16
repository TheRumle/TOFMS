using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Error;

public class UndefinedPartTypeException : InvalidJsonTofmException
{
    private readonly MoveActionStructure _moveActionStructure;
    private readonly string _undeclaredPartType;

    public UndefinedPartTypeException(MoveActionStructure moveActionStructure, string undeclaredPartType)
    {
        _moveActionStructure = moveActionStructure;
        this._undeclaredPartType = undeclaredPartType;
    }
    
    public override string ToString()
    {
        return $"Move action {_moveActionStructure.Name} uses part type {_undeclaredPartType} which has not been defined.";
    }
    public override string ErrorCategory { get; } = "Undefined part type";
}