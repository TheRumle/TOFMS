using Newtonsoft.Json;
using Tmpms.Json.Errors;
using Tmpms.Json.Models;
using Tmpms.Json.Validators;

namespace Tmpms.Json;

public class TmpmsJsonInputParser
{
    private readonly JsonSerializerSettings ParseSettings = CreateJsonParserSettings();

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
            
        var errs = _validator.Validate(jsonSystem).ToArray();
        
        if (errs.Any())
            return Task.FromException<TimedMultipartSystem>(GetValidationErrorMessage(errs));


        return Task.FromResult(new TmpmsFromJsonFactory(jsonSystem.Parts).Create(jsonSystem));
    }
    
    private static ArgumentException GetValidationErrorMessage(InvalidJsonTmpmsException[] invalidJsonTofmExceptions)
    {
        var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
        return new ArgumentException(message);
    }
}