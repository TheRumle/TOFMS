using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Validators;

public interface INamingValidator : IValidator<IEnumerable<LocationDefinition>, IEnumerable<MoveActionDefinition>>
{
}