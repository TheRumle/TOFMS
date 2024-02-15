using Tmpms.Json.Errors;
using Tmpms.Json.Models;

namespace Tmpms.Json.Validators.ValidationFunctions;

internal static class MoveActionValidation
{
    public static IEnumerable<InvalidJsonTmpmsException> ValidatePartTypesAreDefined(IEnumerable<MoveActionDefinition> values,
        IEnumerable<string> definedPartTypes)
    {

        var errors = new List<InvalidJsonTmpmsException>();
        foreach (var moveActionDefinition in values)
        {
            var errs = moveActionDefinition.Parts
                .Where(value => string.IsNullOrWhiteSpace(value.Key) || !definedPartTypes.Contains(value.Key))
                .Select(e => new InvalidPartTypeException(e.Key, moveActionDefinition));
            errors.AddRange(errs);
        }

        return errors;
    }
    
    public static IEnumerable<InvalidJsonTmpmsException> ValidateLocationsAreDefined(IEnumerable<MoveActionDefinition> values,
        IEnumerable<LocationDefinition> locations)
    {
        var locationDefinitions = locations as LocationDefinition[] ?? locations.ToArray();
        var locationsByName = new Dictionary<string, LocationDefinition>(locationDefinitions.Select(e=>KeyValuePair.Create(e.Name,e)));

        var errs = new List<InvalidJsonTmpmsException>();
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