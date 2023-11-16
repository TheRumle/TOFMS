using System.Collections.Concurrent;
using Common.JsonTofms.ConsistencyCheck;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.JsonTofms.Models;
using Common.Move;
using Newtonsoft.Json;

namespace Common.JsonTofms;

public class TofmSystemValidator : IValidator<TofmSystem>
{
    private readonly IValidator<IEnumerable<LocationStructure>> _locationValidator;
    private readonly IValidator<IEnumerable<MoveActionStructure>, IEnumerable<LocationStructure>> _moveActionValidator;


    public TofmSystemValidator(IValidator<IEnumerable<LocationStructure>> locationValidator, IValidator<IEnumerable<MoveActionStructure>, IEnumerable<LocationStructure>> moveActionValidator)
    {
        _locationValidator = locationValidator;
        _moveActionValidator = moveActionValidator;
    }
    
    public IEnumerable<InvalidJsonTofmException> Validate(TofmSystem system)
    {
        var errs = new List<InvalidJsonTofmException>();
        foreach (var component in system.Components)
        {
            var locationErrsTask = _locationValidator.Validate(component.Locations);
            var moveActionErrsTask = _moveActionValidator.Validate(component.Moves, component.Locations);
            errs.AddRange(locationErrsTask);
            errs.AddRange(moveActionErrsTask);
        }
        return errs;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(TofmSystem system)
    {
        var errs = new ConcurrentBag<InvalidJsonTofmException>();
        var validationTasks = new List<Task>();
        foreach (var component in system.Components)
        {
            Task<IEnumerable<InvalidJsonTofmException>> locationErrsTask =
                _locationValidator.ValidateAsync(component.Locations);
            Task<IEnumerable<InvalidJsonTofmException>> moveActionErrsTask =
                _moveActionValidator.ValidateAsync(component.Moves, component.Locations);

            // Add continuation to handle exceptions and add them to the errs bag
            validationTasks.Add(locationErrsTask.ContinueWith(AddErrorConcurrently(errs)));
            validationTasks.Add(moveActionErrsTask.ContinueWith(AddErrorConcurrently(errs)));
            await Task.WhenAll(validationTasks);
        }

        return errs;
    }
    
    private static Action<Task<IEnumerable<InvalidJsonTofmException>>> AddErrorConcurrently(ConcurrentBag<InvalidJsonTofmException> errs)
    {
        return task =>
        {
            if (task.Result.Any())
            {
                foreach (var exception in task.Result)
                {
                    errs.Add(exception);
                }
            }
        };
    }
}

public class JsonTofmParser
{
    private readonly IValidator<TofmSystem> _validator;
    private readonly ITofmsFactory _systemFactory;


    public JsonTofmParser(IValidator<TofmSystem> validator, ITofmsFactory systemFactory)
    {
        _validator = validator;
        _systemFactory = systemFactory;
    }

    public async Task<IEnumerable<MoveAction>> ParseTofmsSystemJsonString(string jsonString)
    {
        TofmSystem? system = JsonConvert.DeserializeObject<TofmSystem>(jsonString);
        if (system is null) throw new ArgumentException($"The inputted string is not of the same format as {typeof(TofmSystem).FullName}.");

        var errs = await _validator.ValidateAsync(system);
        var invalidJsonTofmExceptions = errs as InvalidJsonTofmException[] ?? errs.ToArray();
        if (invalidJsonTofmExceptions.Any()) throw new ArgumentException(new ErrorPrinter(invalidJsonTofmExceptions).ToErrorString());

        return _systemFactory.CreateMoveActions(system);
    }




}