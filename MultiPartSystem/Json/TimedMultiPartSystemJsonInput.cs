using Tmpms.Common.JsonTofms.Models;

namespace Tmpms.Common.Json;

public record TimedMultiPartSystemJsonInput(
    List<LocationDefinition> LocationDeclarations,
    List<MoveActionDefinition> Actions,
    Dictionary<string, IEnumerable<string>> Journeys,
    List<string> Parts)
{
}