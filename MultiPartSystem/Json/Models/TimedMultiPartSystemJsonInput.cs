namespace Tmpms.Common.Json.Models;

public record TimedMultiPartSystemJsonInput(
    List<LocationDefinition> LocationDeclarations,
    List<MoveActionDefinition> Actions,
    Dictionary<string, IEnumerable<string>> Journeys,
    List<string> Parts)
{
}