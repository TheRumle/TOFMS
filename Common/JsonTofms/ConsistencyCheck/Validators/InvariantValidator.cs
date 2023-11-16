using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public class InvariantValidator : IValidator<IEnumerable<InvariantDefinition>>
{
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<InvariantDefinition> values)
    {
        var result = new List<InvalidJsonTofmException>();
        foreach (var invariant in values)
            if (invariant.Max < invariant.Min) result.Add(new InvalidInvariantException(invariant));

        return result;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<InvariantDefinition> values)
    {
        return await Task.Run(() => Validate(values));
    }
}