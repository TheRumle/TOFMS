using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public interface INamingValidator : IValidator<IEnumerable<LocationDefinition>, IEnumerable<MoveActionDefinition>>
{
}