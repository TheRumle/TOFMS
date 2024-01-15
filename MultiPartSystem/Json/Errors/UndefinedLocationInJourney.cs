namespace Tmpms.Common.Json.Errors;

public class UndefinedLocationInJourney : InvalidJsonTofmException
{
    private readonly string part;
    private readonly string location;

    public UndefinedLocationInJourney(string partType, string locationName)
    {
        this.part = partType;
        this.location = locationName;
    }
    
    public override string ErrorCategory { get; } = "Journey";
    public override string ToString()
    {
        return $"The journey for {part} contains the location '{location}', which has not been declared.";
    }
}