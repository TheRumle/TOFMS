using Tmpms.Common.Json.Errors;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json.Validators.ValidationFunctions;

public static class LocationValidation
{
    public static IEnumerable<InvalidJsonTofmException> CapacityValidation(IEnumerable<LocationDefinition> locations)
    {
        return locations
            .Where(e => e.Capacity < 1)
            .Select(e => new InvalidCapacityException(e));
    }
    
    public static IEnumerable<InvalidJsonTofmException> InvariantValidation(IEnumerable<LocationDefinition> locations)
    {
        var errs = new List<InvalidJsonTofmException>();
        foreach (var location in locations)
        {
            foreach (var invariant in location.Invariants)
            {
                if (invariant.Max < invariant.Min)
                    errs.Add(new InvalidInvariantException(invariant));
            
                if (String.IsNullOrWhiteSpace(invariant.Part))
                    errs.Add(new PartTypeNameEmptyException<InvariantDefinition>(invariant));
            }
        }

        return errs;
    }
    public static IEnumerable<InvalidJsonTofmException> DuplicateLocationValidation(IEnumerable<LocationDefinition> locations)
    {
        var duplicates = GetDuplicateLocationDefinitions(locations);
        
        var errors = new List<InvalidJsonTofmException>();
        foreach (var duplicate in duplicates)
            errors.Add(new DuplicateDefinitionError<LocationDefinition>(duplicate));
        
        return errors;
    }
    
    private static IGrouping<string, LocationDefinition>[] GetDuplicateLocationDefinitions(IEnumerable<LocationDefinition> locations)
    {
        var a = locations.GroupBy(e => e.Name)
            .Where(gr => gr.Count() > 1).ToArray();
        return a;
    }
}