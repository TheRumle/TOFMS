using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.JsonTofms.Models;
using Common.Move;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class JsonTofmToDomainTofmParser
{
    private readonly IValidator<TofmSystem> _validator;
    private readonly ITofmsFactory _systemFactory;


    public JsonTofmToDomainTofmParser(IValidator<TofmSystem> validator, ITofmsFactory systemFactory)
    {
        _validator = validator;
        _systemFactory = systemFactory;
    }

    public async Task<IEnumerable<MoveAction>> ParseTofmsSystemJsonString(string jsonString)
    {
        TofmSystem? system = JsonConvert.DeserializeObject<TofmSystem>(jsonString);
        if (system is null) throw new ArgumentException($"The inputted string is not of the same format as {typeof(TofmSystem).FullName}.");

        IEnumerable<InvalidJsonTofmException> errs = await _validator.ValidateAsync(system);
        
        InvalidJsonTofmException[] invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        if (invalidJsonTofmExceptions.Any())
        {
            var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
            throw new AggregateException(message, invalidJsonTofmExceptions);
        }

        return _systemFactory.CreateMoveActions(system);
    }
}