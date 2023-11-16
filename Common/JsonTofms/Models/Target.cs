namespace Common.JsonTofms.Models;

public record Target
{
    public List<LocationStructure> Locations { get; set; }
    public List<MoveActionStructure> MoveActions { get; set; }
    public List<String> Parts { get; set; }
}