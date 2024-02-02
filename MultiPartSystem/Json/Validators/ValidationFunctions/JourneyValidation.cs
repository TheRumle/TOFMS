using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Validators.ValidationFunctions;

internal static class JourneyValidation
{
    public static IEnumerable<InvalidJsonTmpmsException> MustBeProcessingLocations(Dictionary<string, IEnumerable<string>> journey,
        Dictionary<string, LocationDefinition> locationsByName )
    {
        var errors = new List<InvalidJsonTmpmsException>();
        foreach (var kvp in journey)
        {
            var part = kvp.Key;
            var journeyForPart = journey[part];

            var locationsInJourney = locationsByName
                .Where(e => journeyForPart.Contains(e.Key))
                .Select(e=>e.Value);

            foreach (var loc in locationsInJourney)
                if(!loc.IsProcessing) 
                    errors.Add(new JourneyMustBeProcessingLocationException(part, loc));
        }

        return errors;
    }
    
    public static IEnumerable<InvalidJsonTmpmsException> MustBeDefinedLocations(Dictionary<string, IEnumerable<string>> journey,
        Dictionary<string, LocationDefinition> locationsByName )
    {
        var errors = new List<InvalidJsonTmpmsException>();
        foreach (var kvp in journey)
        {
            var part = kvp.Key;
            var journeyForPart = journey[part];

            foreach (var locationName in journeyForPart)
                if (!locationsByName.ContainsKey(locationName))
                    errors.Add(new UndefinedLocationInJourney(part, locationName));
        }

        return errors;
    }
    
    public static IEnumerable<InvalidJsonTmpmsException> MustBeDefinedPartType(Dictionary<string, IEnumerable<string>> journey,
        IEnumerable<string> declaredParts)
    {
        var errors = new List<InvalidJsonTmpmsException>();
        foreach (var kvp in journey)
        {
            if (!declaredParts.Contains(kvp.Key))
                errors.Add(new JourneyUndefinedPartType(kvp.Key));
        }

        return errors;
    }
}