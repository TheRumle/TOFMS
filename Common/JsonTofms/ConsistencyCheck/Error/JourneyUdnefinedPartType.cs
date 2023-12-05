namespace Tofms.Common.JsonTofms.ConsistencyCheck.Error;

public class JourneyUdnefinedPartType : InvalidJsonTofmException
{
    public override string ErrorCategory { get; } = "Journey";
    public override string ToString()
    {
        return $"The journey contained part types that were not defined in the system";
    }
}