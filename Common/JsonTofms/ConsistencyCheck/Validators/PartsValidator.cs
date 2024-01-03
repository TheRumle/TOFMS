using Tmpms.Common.JsonTofms.ConsistencyCheck.Error;
using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Validators;

public class PartsValidator : IValidator<IEnumerable<MoveActionDefinition>, IEnumerable<string>>
{
    private List<InvalidJsonTofmException> errs = new();
    private void ValidatePartNamesAreDefined(MoveActionDefinition action, IEnumerable<string> parts)
    {
        IEnumerable<string> partNames = action.Parts.Select(e => e.PartType);
        var enumerable = parts as string[] ?? parts.ToArray();
        foreach (string partName in partNames)
            if (enumerable.All(e => partName != e)) 
                errs.Add(new UndefinedPartTypeException(action,partName));
        
    }

    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<MoveActionDefinition> actions, IEnumerable<string> parts)
    {
        foreach (var action in actions)
            ValidatePartNamesAreDefined(action, parts);

        return errs;
    }

    public Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<MoveActionDefinition> values, IEnumerable<string> context)
    {
        return Task.Run(()=>Validate(values,context));
    }
}