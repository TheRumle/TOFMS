using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;
using Tmpms.Common.Json.Validators.ValidationFunctions;

namespace Tmpms.Common.Json.Validators;

public class LocationValidator : JsonValidator<IEnumerable<LocationDefinition>>
{
    public override Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(IEnumerable<LocationDefinition> inputs)
    {
        return new[]
        {
            LocationValidation.DuplicateLocationValidation,
            LocationValidation.InvariantValidation,
            LocationValidation.CapacityValidation
        }.BeginValidationsOver(inputs);
    }
}