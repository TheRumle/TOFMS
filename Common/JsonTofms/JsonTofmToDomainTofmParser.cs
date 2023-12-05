using Newtonsoft.Json;
using Tofms.Common.Factories;
using Tofms.Common.JsonTofms.ConsistencyCheck;
using Tofms.Common.JsonTofms.ConsistencyCheck.Error;
using Tofms.Common.JsonTofms.ConsistencyCheck.Validators;
using Tofms.Common.JsonTofms.Models;
using Tofms.Common.Move;

namespace Tofms.Common.JsonTofms;

public class JsonTofmToDomainTofmParser
{
    private readonly ITofmsFactory _systemFactory;
    private readonly IValidator<TofmJsonSystem> _systemValidator;
    private readonly MissingDefinitionValidator _missingDefinitionValidator = new MissingDefinitionValidator();


    public JsonTofmToDomainTofmParser(IValidator<TofmJsonSystem> systemValidator, ITofmsFactory systemFactory)
    {
        _systemValidator = systemValidator;
        _systemFactory = systemFactory;
    }
    
    public JsonTofmToDomainTofmParser(IValidator<TofmJsonSystem> systemValidator)
    {
        _systemValidator = systemValidator;
        _systemFactory = new MoveActionFactory();
    }

    public async Task<TofmSystem> ParseTofmsSystemJsonString(string jsonString)
    {
        var system = JsonConvert.DeserializeObject<TofmJsonSystem>(jsonString);
        ValidateTopLevel(system);
        await PerformComponentDefinitionValidation(system.Components);
        await PerformSystemValidation(system);
        var moveActions =  _systemFactory.CreateMoveActions(system!);

        var journeys = CreateJourneys(moveActions, system);


        return new TofmSystem()
        {
            Journeys = journeys,
            MoveActions = moveActions,
            Parts = system.Parts!,
        };
    }

    private static Dictionary<string, List<Location>> CreateJourneys(IReadOnlyCollection<MoveAction> moveActions, TofmJsonSystem system)
    {
        var journeys = new Dictionary<string, List<Location>>();

        var processingLocations = moveActions
            .SelectMany(e => e.InvolvedLocations).Where(e => e.IsProcessing);

        foreach (KeyValuePair<string, IEnumerable<string>> jour in system.Journeys)
        {
            var partName = jour.Key;
            var journeyTargetNames = jour.Value.ToList();
            
            foreach (var location in processingLocations)
            {
                if (journeyTargetNames.Contains(location.Name))
                {
                    if (journeys.ContainsKey(location.Name))
                    {
                        journeys[partName].Add(location);
                    }
                    else
                    {
                        journeys.Add(partName, new List<Location>() { location });
                    }
                }
            }
        }

        return journeys;
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
        TofmJsonSystem jsonSystem = new TofmJsonSystem()
        {
            Components = new List<TofmComponent>() { component },
            Parts = new List<string>(parts)
        };
        
        await PerformSystemValidation(jsonSystem);
        return _systemFactory.CreateMoveActions(jsonSystem);
    }

    private async Task PerformSystemValidation(TofmJsonSystem jsonSystem)
    {
        var errs = await _systemValidator.ValidateAsync(jsonSystem);
        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        if (invalidJsonTofmExceptions.Any())
            ThrowErrorMessage(invalidJsonTofmExceptions);
    }

    private static void ThrowErrorMessage(InvalidJsonTofmException[] invalidJsonTofmExceptions)
    {
        var message = new ErrorFormatter(invalidJsonTofmExceptions).ToErrorString();
        throw new Exception(message);
    }
    
    private static void ValidateTopLevel(TofmJsonSystem? system)
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