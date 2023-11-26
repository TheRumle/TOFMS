using Tofms.Common.JsonTofms.Models;

namespace Tofms.Common.JsonTofms.ConsistencyCheck.Validators;

public record MoveActionStructureValidationContext(IEnumerable<LocationDefinition> LocationStructures,
    IEnumerable<string> parts);