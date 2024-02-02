namespace Tmpms.Common.Json.Errors;

public class JourneyUndefinedPartType : InvalidJsonTmpmsException
{
    private readonly string part;

    public JourneyUndefinedPartType(string partType)
    {
        this.part = partType;
    }
    public override string ErrorCategory { get; } = "Journey";
    public override string ToString()
    {
        return $"The journey contained part type '{part}' that were not defined in the system";
    }
}