using Common.Move;

namespace Common.JsonTofms;

public record TOFMSJsonInput
{
    public List<Location> Locations { get; set; }
    public List<MoveAction> MoveActions { get; set; }
}