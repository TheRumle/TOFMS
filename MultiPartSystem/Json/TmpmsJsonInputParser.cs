using Newtonsoft.Json;
using Tmpms.Common.Json.Convertion;
using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Validators;
using Tmpms.Common.Move;

namespace Tmpms.Common.Json;

public class TmpmsJsonInputParser
{
    private readonly JsonSerializerSettings ParseSettings = CreateJsonParserSettings();
    private readonly ITmpmsSystemFactory<TimedMultiPartSystemJsonInput> _systemFactory = new FromJsonFactory();

    private static JsonSerializerSettings CreateJsonParserSettings()
    {
        var settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        return settings;
    }
    
    private readonly TimedMultiPartSystemJsonInputValidator _validator;

    public TmpmsJsonInputParser(TimedMultiPartSystemJsonInputValidator validator)
    {
        this._validator = validator;
    }
    
    
    public Task<ValidatedTofmSystem> ParseTofmsSystemJsonString(string jsonString)
    {
        var jsonSystem = JsonConvert.DeserializeObject<TimedMultiPartSystemJsonInput>(jsonString, ParseSettings);
        if (jsonSystem is null) throw new ApplicationException("There is something entirely wrong with the format of the inputted TMPMS:\n " + jsonString);
        
        var errs = _validator.Validate(jsonSystem).ToArray();
        if (errs.Any()) ThrowErrorMessage(errs);

        return Task.FromResult(_systemFactory.Create(jsonSystem));
    }
    
    private static void ThrowErrorMessage(InvalidJsonTofmException[] invalidJsonTofmExceptions)
    {
        var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
        throw new Exception(message);
    }
}

internal class FromJsonFactory : ITmpmsSystemFactory<TimedMultiPartSystemJsonInput>
{
    public ValidatedTofmSystem Create(TimedMultiPartSystemJsonInput structure)
    {
        return new ValidatedTofmSystem()
        {
            Journeys = CreateJourneys(structure),
            MoveActions = CreateMoveActions(structure),
            Parts = structure.Parts
        };
    }

    private IEnumerable<MoveAction> CreateMoveActions(TimedMultiPartSystemJsonInput structure)
    {
        var locations = structure.LocationDeclarations.Select(e => e.ToDomain());
        return structure.Actions.Select(e => e.ToDomain(locations));
    }

    private Dictionary<string, List<Location>> CreateJourneys(TimedMultiPartSystemJsonInput structure)
    {

        Dictionary<string, List<Location>> journey = new();
        foreach (var kvp in structure.Journeys)
        {
            var partType = kvp.Key;
            var locationNames = kvp.Value;
            
            var locations = structure
                .LocationDeclarations
                .Where(e => locationNames.Contains(e.Name))
                .Select(jsonDefinition => jsonDefinition.ToDomain())
                .ToList();
            journey.Add(partType, locations);
        }

        return journey;
    }
}

internal interface ITmpmsSystemFactory<T>
{
    ValidatedTofmSystem Create(T structure);
}