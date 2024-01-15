using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json.Validators;

public class JourneyValidator : JsonValidator<Dictionary<string, IEnumerable<string>>>
{
    private Dictionary<string, LocationDefinition> _locationByName;
    private readonly IEnumerable<string> _partTypes;

    public JourneyValidator(IEnumerable<LocationDefinition> locationDefinitions, IEnumerable<string> partTypes)
    {
        _locationByName = new Dictionary<string, LocationDefinition>(locationDefinitions.Select(e=>KeyValuePair.Create(e.Name, e)));
        _partTypes = partTypes;
    }
    
    public override Task<IEnumerable<InvalidJsonTofmException>>[] ValidationTasksFor(Dictionary<string, IEnumerable<string>> journey)
    {
        var validateProcessingLocationOnly = (Dictionary<string, IEnumerable<string>> journey) =>
            JourneyValidation.MustBeProcessingLocations(journey, _locationByName);
        
        var validateLocationsMustBeDefined = (Dictionary<string, IEnumerable<string>> journey) =>
            JourneyValidation.MustBeDefinedLocations(journey, _locationByName);
        
        var validatePartsMustBeDefined = (Dictionary<string, IEnumerable<string>> journey) =>
            JourneyValidation.MustBeDefinedPartType(journey,_partTypes);
        
        return new[] { validateProcessingLocationOnly, validateLocationsMustBeDefined, validatePartsMustBeDefined}
            .BeginValidationsOver(journey);
    }
    
}