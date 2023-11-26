using Tofms.Common.JsonTofms.Models;

namespace Tofms.Common.JsonTofms.ConsistencyCheck.Error;

public class UndefinedPartTypeException : InvalidJsonTofmException
{
    private readonly MoveActionDefinition _moveActionDefinition;
    private readonly string _undeclaredPartType;

    public UndefinedPartTypeException(MoveActionDefinition moveActionDefinition, string undeclaredPartType)
    {
        _moveActionDefinition = moveActionDefinition;
        _undeclaredPartType = undeclaredPartType;
    }

    public override string ErrorCategory { get; } = "Undefined part type";

    public override string ToString()
    {
        return
            $"Move action {_moveActionDefinition.Name} uses part type {_undeclaredPartType} which has not been defined.";
    }
}