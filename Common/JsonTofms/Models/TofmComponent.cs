namespace Tofms.Common.JsonTofms.Models;

public record TofmComponent
{
    public List<LocationDefinition> Locations { get; set; }
    public List<MoveActionDefinition> Moves { get; set; }
    public string Name { get; set; }
}

public class TofmSystem
{
    public Dictionary<Location, IEnumerable<Location>> Journeys { get; set; }
    public List<TofmComponent> Components { get; set; }
    public List<string> Parts { get; set; }
}