using Common.JsonTofms.Models;

namespace Common.JsonTofms.ConsistencyCheck.Validators;

public record MoveActionStructureValidationContext(IEnumerable<LocationDefinition> LocationStructures,
    IEnumerable<string> parts);