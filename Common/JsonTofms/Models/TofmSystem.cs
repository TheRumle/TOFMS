namespace Tmpms.Common.JsonTofms.Models;

public class TofmSystem
{
    public Dictionary<Location, List<Location>> Journeys { get; set; }
    public List<TofmComponent> Components { get; set; }
    public List<string> Parts { get; set; }
}