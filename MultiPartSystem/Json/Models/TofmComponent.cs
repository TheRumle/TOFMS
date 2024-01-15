namespace Tmpms.Common.Json.Models;

public record TofmComponent
{
    public List<LocationDefinition> Locations { get; set; }
    public List<MoveActionDefinition> Moves { get; set; }
    public string Name { get; set; }
}