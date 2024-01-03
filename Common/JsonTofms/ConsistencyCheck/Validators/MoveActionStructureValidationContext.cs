using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.JsonTofms.ConsistencyCheck.Validators;


public record MoveActionStructureValidationContext(IEnumerable<LocationDefinition> LocationStructures,
    IEnumerable<string> parts);