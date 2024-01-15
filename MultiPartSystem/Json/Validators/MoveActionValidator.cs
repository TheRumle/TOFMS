using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;
using Tmpms.Common.Json.Validators.ValidationFunctions;

namespace Tmpms.Common.Json.Validators;

public class MoveActionValidator : JsonValidator<IEnumerable<MoveActionDefinition>>
{
    private readonly IEnumerable<LocationDefinition> _locations;
    private readonly IEnumerable<string> _parts;

    public MoveActionValidator(IEnumerable<LocationDefinition> validatedLocations, IEnumerable<string> definedParts)
    {
        _locations = validatedLocations;
        _parts = definedParts;
    }
    

    public override Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(IEnumerable<MoveActionDefinition> inputs)
    {
        var validateLocationAreDefined = (IEnumerable<MoveActionDefinition> actions) =>
            MoveActionValidation.ValidateLocationsAreDefined(actions, _locations);
        
        var validatePartsAreDefined = (IEnumerable<MoveActionDefinition> actions) =>
            MoveActionValidation.ValidatePartTypesAreDefined(actions,_parts);
        
        return new[]
        {
            validateLocationAreDefined,
            validatePartsAreDefined
        }.BeginValidationsOver(inputs);
    }
}