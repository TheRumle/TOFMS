namespace Common.JsonTofms.Models;

public record TofmComponent
{
    public List<LocationStructure> Locations { get; set; }
    public List<MoveActionStructure> Moves { get; set; }
    public string Name { get; set; }
}

public record TofmSystem
{
    public List<String> Parts { get; set; }
    public List<TofmComponent> Components { get; set; }
}