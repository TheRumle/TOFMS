using Tmpms.Json.Errors;
using Tmpms.Json.Models;
using Tmpms.Json.Validators.ValidationFunctions;

namespace Tmpms.Json.Validators;

internal class LocationDefinitionValidator : JsonValidator<IEnumerable<LocationDefinition>>
{
    private readonly string[] _partTypes;

    public LocationDefinitionValidator(string[] partTypes)
    {
        _partTypes = partTypes;
    }
    
    public override Task<IEnumerable<InvalidJsonTmpmsException>>[] ValidationTasksFor(IEnumerable<LocationDefinition> inputs)
    {
        return new[]
        {
            LocationValidation.DuplicateLocationValidation,
            LocationValidation.InvariantAreValidValidation,
            LocationValidation.CapacityValidation,
            LocationValidation.NoInfiniteVariantForProcessingLocations,
            e=>LocationValidation.InvariantAreOverDefinedPartsLocationValidation(e, _partTypes)
        }.BeginValidationsOver(inputs);
    }
}