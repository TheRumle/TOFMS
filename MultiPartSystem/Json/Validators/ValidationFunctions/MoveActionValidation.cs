using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json.Validators.ValidationFunctions;

public static class MoveActionValidation
{
    public static IEnumerable<InvalidJsonTofmException> ValidatePartTypesAreDefined(IEnumerable<MoveActionDefinition> values,
        IEnumerable<string> definedPartTypes)
    {
        return values
            .SelectMany(e => e.Parts)
            .Where(value => string.IsNullOrWhiteSpace(value.PartType) || !definedPartTypes.Contains(value.PartType))
            .Select(e => new PartTypeNameEmptyException<PartConsumptionDefinition>(e));
    }
    
    public static IEnumerable<InvalidJsonTofmException> ValidateLocationsAreDefined(IEnumerable<MoveActionDefinition> values,
        IEnumerable<LocationDefinition> locations)
    {
        var locationDefinitions = locations as LocationDefinition[] ?? locations.ToArray();
        var locationsByName = new Dictionary<string, LocationDefinition>(locationDefinitions.Select(e=>KeyValuePair.Create(e.Name,e)));

        var errs = new List<InvalidJsonTofmException>();
        foreach (var structure in values)
        {
            if (!locationsByName.Keys.Contains(structure.From))
                errs.Add(new UndefinedLocationException(structure, structure.From));
            if (!locationsByName.Keys.Contains(structure.To)) 
                errs.Add(new UndefinedLocationException(structure, structure.To));
        }

        return errs;
    }
}