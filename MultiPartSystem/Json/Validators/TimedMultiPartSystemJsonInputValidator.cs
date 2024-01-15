using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;

namespace Tmpms.Common.Json.Validators;

public class TimedMultiPartSystemJsonInputValidator : JsonValidator<TimedMultiPartSystemJsonInput>
{
    private readonly LocationValidator _locationValidator = new();
    private readonly MoveActionValidator _moveActionValidator;

    public override Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(TimedMultiPartSystemJsonInput inputs)
    {
        var moveActionValidator = new MoveActionValidator(inputs.LocationDeclarations, inputs.Parts)
            .ValidationTasksFor(inputs.Actions);
        
        var locationsValidators = new LocationValidator()
            .ValidationTasksFor(inputs.LocationDeclarations);
        
        var journeyValidator = new JourneyValidator(inputs.LocationDeclarations, inputs.Parts)
            .ValidationTasksFor(inputs.Journeys);
        
        var a =  new List<Task<IEnumerable<InvalidJsonTofmException>>>(moveActionValidator.Length + locationsValidators.Length + journeyValidator.Length);
        a.AddRange(moveActionValidator);
        a.AddRange(locationsValidators);
        a.AddRange(journeyValidator);
        return a.ToArray();
    }
}