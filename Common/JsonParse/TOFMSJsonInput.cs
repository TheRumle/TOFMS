using Common.Move;

namespace Common.JsonParse;

public record TOFMSJsonInput
{
    public List<Location> Locations { get; set; }
    public List<MoveAction> MoveActions { get; set; }
}