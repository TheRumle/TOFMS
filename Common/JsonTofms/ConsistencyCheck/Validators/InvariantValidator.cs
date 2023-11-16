using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public class InvariantValidator : IValidator<InvariantStructure>
{
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<InvariantStructure> values)
    {
        var result = new List<InvalidJsonTofmException>();
        foreach (var invariant in values)
            if (invariant.Max < invariant.Min) result.Add(new InvalidInvariantException(invariant));

        return result;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<InvariantStructure> values)
    {
        return await Task.Run(() => Validate(values));
    }
}