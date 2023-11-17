using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public class LocationValidator : IValidator<IEnumerable<LocationDefinition>>
{
    private readonly InvariantValidator _invariantValidator;

    public LocationValidator(InvariantValidator invariantValidator)
    {
        _invariantValidator = invariantValidator;
    }

    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<LocationDefinition> values)
    {
        var errs = new List<InvalidJsonTofmException>();
        Dictionary<string, LocationDefinition> dictionary = new();
        var locationStructures = values as LocationDefinition[] ?? values.ToArray();

        foreach (var structure in locationStructures)
            ValidateCapacities(structure, dictionary, errs);

        var invariantErrs = _invariantValidator.Validate(locationStructures.SelectMany(e => e.Invariants));
        errs.AddRange(invariantErrs);

        return errs;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<LocationDefinition> values)
    {
        return await Task.Run(() => Validate(values));
    }

    private static void ValidateCapacities(LocationDefinition definition,
        Dictionary<string, LocationDefinition> dictionary, List<InvalidJsonTofmException> errs)
    {
        (var name, var newCap) = (definition.Name, definition.Capacity);

        if (!dictionary.ContainsKey(name)) return;
        var existsingStructure = dictionary[name];
        if (existsingStructure.Capacity != newCap)
            errs.Add(new InconsistentCapacityException(existsingStructure, newCap));
    }
}