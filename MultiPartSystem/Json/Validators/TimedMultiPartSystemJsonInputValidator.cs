using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;

namespace Tmpms.Common.Json.Validators;

public class TimedMultiPartSystemJsonInputValidator : JsonValidator<TimedMultiPartSystemJsonInput>
{
    public override Task<IEnumerable<InvalidJsonTmpmsException>>[] ValidationTasksFor(TimedMultiPartSystemJsonInput inputs)
    {
        var moveActionValidator = new MoveActionValidator(inputs.LocationDeclarations, inputs.Parts)
            .ValidationTasksFor(inputs.Actions);
        
        var locationsValidators = new LocationValidator()
            .ValidationTasksFor(inputs.LocationDeclarations);
        
        var journeyValidator = new JourneyValidator(inputs.LocationDeclarations, inputs.Parts)
            .ValidationTasksFor(inputs.Journeys);

        var namingValidator = new NamingValidator()
            .ValidationTasksFor(inputs.LocationDeclarations, inputs.Actions);
        
        var results =  new List<Task<IEnumerable<InvalidJsonTmpmsException>>>(moveActionValidator.Length + locationsValidators.Length + journeyValidator.Length);
        results.AddRange(moveActionValidator);
        results.AddRange(locationsValidators);
        results.AddRange(journeyValidator);
        results.AddRange(namingValidator);
        return results.ToArray();
    }
}