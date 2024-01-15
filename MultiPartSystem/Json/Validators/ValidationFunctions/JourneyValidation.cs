using Tmpms.Common.Json.Errors;
using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json.Validators.ValidationFunctions;

public static class JourneyValidation
{
    public static IEnumerable<InvalidJsonTofmException> MustBeProcessingLocations(Dictionary<string, IEnumerable<string>> journey,
        Dictionary<string, LocationDefinition> locationsByName )
    {
        var errors = new List<InvalidJsonTofmException>();
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
    
    public static IEnumerable<InvalidJsonTofmException> MustBeDefinedLocations(Dictionary<string, IEnumerable<string>> journey,
        Dictionary<string, LocationDefinition> locationsByName )
    {
        var errors = new List<InvalidJsonTofmException>();
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
    
    public static IEnumerable<InvalidJsonTofmException> MustBeDefinedPartType(Dictionary<string, IEnumerable<string>> journey,
        IEnumerable<string> declaredParts)
    {
        var errors = new List<InvalidJsonTofmException>();
        foreach (var kvp in journey)
        {
            if (!declaredParts.Contains(kvp.Key))
                errors.Add(new JourneyUndefinedPartType(kvp.Key));
        }

        return errors;
    }
}