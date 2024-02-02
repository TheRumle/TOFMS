using Newtonsoft.Json;
using Tmpms.Common.Json.Errors;
using Tmpms.Common.Json.Models;
using Tmpms.Common.Json.Validators;

namespace Tmpms.Common.Json;

public class TmpmsJsonInputParser
{
    private readonly JsonSerializerSettings ParseSettings = CreateJsonParserSettings();
    private readonly ITmpmsSystemFactory<TimedMultiPartSystemJsonInput> _systemFactory = new TmpmsFromJsonFactory();

    private static JsonSerializerSettings CreateJsonParserSettings()
    {
        var settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        return settings;
    }
    
    private readonly  IValidator<TimedMultiPartSystemJsonInput>  _validator;

    public TmpmsJsonInputParser(IValidator<TimedMultiPartSystemJsonInput> validator)
    {
        this._validator = validator;
    }
    
    
    public Task<TimedMultipartSystem> ParseTofmsSystemJsonString(string jsonString)
    {
        var jsonSystem = JsonConvert.DeserializeObject<TimedMultiPartSystemJsonInput>(jsonString, ParseSettings);
        if (jsonSystem is null)
            throw new ApplicationException("There is something entirely wrong with the format of the inputted TMPMS:\n " + jsonString);
        
        if (jsonSystem.LocationDeclarations.All(e => e.Name.ToLower() != "END"))
            jsonSystem.LocationDeclarations.Add(LocationDefinition.EndLocation(jsonSystem.Parts));
            
        var errs = _validator.Validate(jsonSystem).ToArray();
        
        if (errs.Any())
            return Task.FromException<TimedMultipartSystem>(GetValidationErrorMessage(errs));

        return Task.FromResult(_systemFactory.Create(jsonSystem));
    }
    
    private static ArgumentException GetValidationErrorMessage(InvalidJsonTmpmsException[] invalidJsonTofmExceptions)
    {
        var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
        return new ArgumentException(message);
    }
}