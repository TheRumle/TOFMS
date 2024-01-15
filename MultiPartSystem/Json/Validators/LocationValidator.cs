using Tmpms.Common.Json.Validators.ValidationFunctions;
using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json.Validators;

public class LocationValidator : JsonValidator<IEnumerable<LocationDefinition>>
{
    public override Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasks(IEnumerable<LocationDefinition> inputs)
    {
        return new[]
        {
            LocationValidation.DuplicateLocationValidation,
            LocationValidation.InvariantValidation,
            LocationValidation.CapacityValidation
        }.BeginValidationsOver(inputs);
    }
}