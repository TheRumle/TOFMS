using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;
using Tmpms.Common.Json.Validators.ValidationFunctions;

namespace Tmpms.Common.Json.Validators;

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
            e=>LocationValidation.InvariantAreOverDefinedPartsLocationValidation(e, _partTypes)
        }.BeginValidationsOver(inputs);
    }
}