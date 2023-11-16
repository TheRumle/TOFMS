using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public class LocationValidator : IValidator<LocationStructure>
{
    private readonly InvariantValidator _invariantValidator;

    public LocationValidator(InvariantValidator invariantValidator)
    {
        _invariantValidator = invariantValidator;
    }
    
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<LocationStructure> values)
    {
        var errs = new List<InvalidJsonTofmException>();
        Dictionary<string, LocationStructure> dictionary = new();
        LocationStructure[] locationStructures = values as LocationStructure[] ?? values.ToArray();
        
        foreach (var structure in locationStructures)
            ValidateCapacities(structure, dictionary, errs);

        IEnumerable<InvalidJsonTofmException> invariantErrs = _invariantValidator.Validate(locationStructures.SelectMany(e => e.Invariants));
        errs.AddRange(invariantErrs);

        return errs;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<LocationStructure> values)
    {
        return await Task.Run(() => Validate(values));
    }

    private static void ValidateCapacities(LocationStructure structure, Dictionary<string, LocationStructure> dictionary, List<InvalidJsonTofmException> errs)
    {
        (string name, int newCap) = (structure.Name, structure.Capacity);
        if (!dictionary.ContainsKey(name)) return;
        var existsingStructure = dictionary[name];
        if (existsingStructure.Capacity != newCap)
            errs.Add(new InconsistentCapacityException(existsingStructure, newCap));
    }
}