using Tmpms.Json.Models;

namespace Tmpms.Json.Errors;

public class InvalidPartTypeException : InvalidJsonTmpmsException
{
    private readonly string partType;
    private readonly MoveActionDefinition context;
    public override string ErrorCategory { get; } = "Invalid part type";

    public InvalidPartTypeException(string partType, MoveActionDefinition context)
    {
        this.partType = partType;
        this.context = context;
    }
    public override string ToString()
    {
       
        return $"{context.Name} is defined using an unspecified part type (parts definitions): {partType}";
    }
}