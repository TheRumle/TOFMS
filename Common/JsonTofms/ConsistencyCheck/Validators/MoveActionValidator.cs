using Common.JsonTofms.ConsistencyCheck.Error;
using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public class MoveActionValidator : IValidator<MoveActionStructure, IEnumerable<LocationStructure>>
{
    public IEnumerable<InvalidJsonTofmException> Validate(IEnumerable<MoveActionStructure> values, IEnumerable<LocationStructure> context)
    {
       
        IEnumerable<string> locationNames = context.Select(e => e.Name);
        List<InvalidJsonTofmException> errs = new List<InvalidJsonTofmException>();
        foreach (MoveActionStructure structure in values)
        {
            var enumerable = locationNames as string[] ?? locationNames.ToArray();
            if (!enumerable.Contains(structure.From)) errs.Add(new UndefinedLocationException(structure, structure.From));
            if (!enumerable.Contains(structure.To)) errs.Add(new UndefinedLocationException(structure, structure.To));
        }

        return errs;
    }

    public async Task<IEnumerable<InvalidJsonTofmException>> ValidateAsync(IEnumerable<MoveActionStructure> values, IEnumerable<LocationStructure> context)
    {
        return await Task.Run(() => Validate(values, context));
    }
}