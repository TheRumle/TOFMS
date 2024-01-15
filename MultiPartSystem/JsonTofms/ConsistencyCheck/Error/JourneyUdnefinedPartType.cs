namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

public class JourneyUdnefinedPartType : InvalidJsonTofmException
{
    private readonly string part;

    public JourneyUdnefinedPartType(string partType)
    {
        this.part = partType;
    }
    public override string ErrorCategory { get; } = "Journey";
    public override string ToString()
    {
        return $"The journey contained part type '{part}' that were not defined in the system";
    }
}