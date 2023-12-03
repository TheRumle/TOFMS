using Tofms.Common.JsonTofms.ConsistencyCheck.Error;
using Tofms.Common.JsonTofms.Models;

namespace Tofms.Common.JsonTofms.ConsistencyCheck.Validators;

public class InvariantValidator : IValidator<IEnumerable<InvariantDefinition>>
{
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<InvariantDefinition> values)
    {
        var errs = new List<InvalidJsonTofmException>();
        foreach (var invariant in values)
        { 
            if (invariant.Max < invariant.Min)
                errs.Add(new InvalidInvariantException(invariant));
            
            if (String.IsNullOrWhiteSpace(invariant.Part))
                errs.Add(new PartTypeNameEmptyException<InvariantDefinition>(invariant));
        }

        return errs;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<InvariantDefinition> values)
    {
        return await Task.Run(() => Validate(values));
    }
}