using Common;
using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Validators.ValidationFunctions;

internal static class LocationValidation
{
    public static IEnumerable<InvalidJsonTmpmsException> CapacityValidation(IEnumerable<LocationDefinition> locations)
    {
        return locations
            .Where(e => e.Capacity < 1)
            .Select(e => new InvalidCapacityException(e));
    }
    
    public static IEnumerable<InvalidJsonTmpmsException> InvariantAreValidValidation(IEnumerable<LocationDefinition> locations)
    {
        var errs = new List<InvalidJsonTmpmsException>();
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
    
    public static IEnumerable<InvalidJsonTmpmsException> InvariantAreOverDefinedPartsLocationValidation(IEnumerable<LocationDefinition> locations, string[] parts)
    {
        var errs = new List<InvalidJsonTmpmsException>();
        foreach (var loc in locations)
        {
            var invalidInvariants = loc.Invariants.Where(inv => !parts.Contains(inv.Part));
            errs.AddRange(invalidInvariants.Select(invariant => new InvalidInvariantException(invariant, invariant.Part)));
        }
        return errs;
    }
    public static IEnumerable<InvalidJsonTmpmsException> DuplicateLocationValidation(IEnumerable<LocationDefinition> locations)
    {
        var duplicates = GetDuplicateLocationDefinitions(locations);
        
        var errors = new List<InvalidJsonTmpmsException>();
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

    public static IEnumerable<InvalidJsonTmpmsException> NoInfiniteVariantForProcessingLocations(IEnumerable<LocationDefinition> locations)
    {
        var invalidLocations =
            locations.Where(e => e.IsProcessing
                                 && e.Invariants
                                     .Any(inv => inv.Max == Infteger.PositiveInfinity));

        foreach (var invalid in invalidLocations)
        {
            yield return InvalidInvariantException.InfiniteProcessing(invalid,
                invalid.Invariants.Where(e => e.Max == Infteger.PositiveInfinity));
        }
    }
}