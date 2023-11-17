using System.Text;
using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.JsonTofms.Models;
using Common.Move;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class JsonTofmToDomainTofmParser
{
    private readonly ITofmsFactory _systemFactory;
    private readonly IValidator<TofmSystem> _systemValidator;
    private readonly MissingDefinitionValidator _missingDefinitionValidator = new MissingDefinitionValidator();


    public JsonTofmToDomainTofmParser(IValidator<TofmSystem> systemValidator, ITofmsFactory systemFactory)
    {
        _systemValidator = systemValidator;
        _systemFactory = systemFactory;
    }

    public async Task<IEnumerable<MoveAction>> ParseTofmsSystemJsonString(string jsonString)
    {
        var system = JsonConvert.DeserializeObject<TofmSystem>(jsonString);
        ValidateTopLevel(system);
        await PerformComponentDefinitionValidation(system.Components);
        await PerformSystemValidation(system);
        return _systemFactory.CreateMoveActions(system!);
    }

    private async Task PerformComponentDefinitionValidation(List<TofmComponent> systemComponents)
    {
        var errs = new List<InvalidJsonTofmException>();
        foreach (var systemComponent in systemComponents) 
            errs.AddRange(await _missingDefinitionValidator.ValidateAsync(systemComponent));
        
        if (errs.Any()) ThrowErrorMessage(errs.ToArray());
    }
    
    private async Task PerformComponentDefinitionValidation(TofmComponent? systemComponent)
    {
        if (systemComponent is null) throw new Exception("Component was malformed");
        var errs = (await _missingDefinitionValidator.ValidateAsync(systemComponent)).ToArray();
        if (errs.Any()) ThrowErrorMessage(errs.ToArray());
    }

    public async Task<IEnumerable<MoveAction>> ParseTofmComponentJsonString(string jsonString)
    {
        var component = JsonConvert.DeserializeObject<TofmComponent>(jsonString);
        await PerformComponentDefinitionValidation(component);
        
        var parts = component!.Moves.SelectMany(e => e.Parts.Select(e=>e.PartType));
        TofmSystem system = new TofmSystem()
        {
            Components = new List<TofmComponent>() { component },
            Parts = new List<string>(parts)
        };
        
        await PerformSystemValidation(system);
        return _systemFactory.CreateMoveActions(system);
    }

    private async Task PerformSystemValidation(TofmSystem system)
    {
        var errs = await _systemValidator.ValidateAsync(system);
        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        if (invalidJsonTofmExceptions.Any())
            ThrowErrorMessage(invalidJsonTofmExceptions);
    }

    private static void ThrowErrorMessage(InvalidJsonTofmException[] invalidJsonTofmExceptions)
    {
        var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
        throw new AggregateException(message, invalidJsonTofmExceptions);
    }
    
    private static void ValidateTopLevel(TofmSystem? system)
    {
        var errors = new List<Exception>();
        if (system is null)
            throw new Exception("The system component was entirely malformed");
        
        if (system.Components is null) 
            errors.Add(new Exception($"The system definition did not have or had malformed component definitions")); 
        
        if (system.Parts is null) 
            errors.Add(new Exception($"The system definition did not have or had malformed parts definitions"));

        if (errors.Any())
            throw new AggregateException(errors);
    }

}