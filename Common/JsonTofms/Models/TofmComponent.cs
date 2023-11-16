namespace Common.JsonTofms.Models;

public record TofmComponent
{
    public List<LocationDefinition> Locations { get; set; }
    public List<MoveActionDefinition> Moves { get; set; }
    public string Name { get; set; }
}