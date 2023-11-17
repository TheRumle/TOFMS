using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.JsonTofms.Models;
using Common.Move;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class JsonTofmToDomainTofmParser
{
    private readonly ITofmsFactory _systemFactory;
    private readonly IValidator<TofmSystem> _systemValidator;


    public JsonTofmToDomainTofmParser(IValidator<TofmSystem> systemValidator, ITofmsFactory systemFactory)
    {
        _systemValidator = systemValidator;
        _systemFactory = systemFactory;
    }

    public async Task<IEnumerable<MoveAction>> ParseTofmsSystemJsonString(string jsonString)
    {
        var system = JsonConvert.DeserializeObject<TofmSystem>(jsonString);
        if (system is null || system.Components is null || system.Parts is null)
            throw new ArgumentException(
                $"The inputted string is not of the same format as {typeof(TofmSystem).FullName}.");

        var errs = await _systemValidator.ValidateAsync(system);

        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        if (invalidJsonTofmExceptions.Any())
        {
            var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
            throw new AggregateException(message, invalidJsonTofmExceptions);
        }

        return _systemFactory.CreateMoveActions(system);
    }
    
    public async Task<IEnumerable<MoveAction>> ParseTofmComponentJsonString(string jsonString)
    {
        var component = JsonConvert.DeserializeObject<TofmComponent>(jsonString);
        if (component is null || component.Locations is null || component.Moves is null)
            throw new ArgumentException(
                $"The inputted string is not of the same format as {typeof(TofmComponent).FullName}.");

        var parts = component.Moves.SelectMany(e => e.Parts.Select(p => p.PartType));
        
        TofmSystem system = new TofmSystem()
        {
            Components = new List<TofmComponent>() { component },
            Parts = new List<string>(parts)
        };
        
        var errs = await _systemValidator.ValidateAsync(system);

        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        if (invalidJsonTofmExceptions.Any())
        {
            var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
            throw new AggregateException(message, invalidJsonTofmExceptions);
        }

        return _systemFactory.CreateMoveActions(system);
    }
}