namespace Common.JsonTofms.Models;

public record TofmComponent
{
    public List<LocationStructure> Locations { get; set; }
    public List<MoveActionStructure> Moves { get; set; }
    public string Name { get; set; }
}