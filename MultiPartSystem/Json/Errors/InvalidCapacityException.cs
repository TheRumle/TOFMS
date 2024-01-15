using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json.Errors;

public class InvalidCapacityException : InvalidJsonTofmException
{
    private readonly LocationDefinition _def;

    public InvalidCapacityException(LocationDefinition locationDefinition)
    {
        _def = locationDefinition;
    }

    public override string ErrorCategory { get; } = "InvalidCapacityException";
    public override string ToString()
    {
        return $"The definition of {_def.Name} has invalid capacity of {_def.Capacity}";
    }
}

public class JourneyMustBeProcessingLocationException : InvalidJsonTofmException
{
    private readonly string part;
    private readonly LocationDefinition location;

    public JourneyMustBeProcessingLocationException(string partType, LocationDefinition locationDefinition)
    {
        this.part = partType;
        this.location = locationDefinition;
    }
    
    public override string ErrorCategory { get; } = "Journey";
    public override string ToString()
    {
        return $"The journey for {part} contains the location {location.Name}, which is not a processing locations. Journeys must contain only processing locations";
    }
}

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