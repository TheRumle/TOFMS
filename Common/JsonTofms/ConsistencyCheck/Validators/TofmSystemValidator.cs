using System.Collections.Concurrent;
using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public class TofmSystemValidator : IValidator<TofmSystem>
{
    private readonly IValidator<IEnumerable<LocationStructure>> _locationValidator;
    private readonly IValidator<IEnumerable<MoveActionStructure>,MoveActionStructureValidationContext> _moveActionValidator;


    public TofmSystemValidator(
        IValidator<IEnumerable<LocationStructure>> locationValidator, 
        IValidator<IEnumerable<MoveActionStructure>,MoveActionStructureValidationContext> moveActionValidator)
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
            var moveActionErrsTask = _moveActionValidator.Validate(component.Moves, new MoveActionStructureValidationContext(component.Locations, system.Parts));
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
                _moveActionValidator.ValidateAsync(component.Moves, new MoveActionStructureValidationContext(component.Locations, system.Parts));

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