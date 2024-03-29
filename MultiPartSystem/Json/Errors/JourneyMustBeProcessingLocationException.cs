﻿using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Errors;

public class JourneyMustBeProcessingLocationException : InvalidJsonTmpmsException
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