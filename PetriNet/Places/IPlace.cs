using TACPN.Colours;
using TACPN.Colours.Type;

namespace TACPN.Places;

public interface IPlace
{
    string Name { get; }
    bool IsCapacityLocation { get;  }
    ColourType ColourType { get; }
    bool IsProcessingPlace { get; }
}
public interface IPlace<T> : IPlace
{
    IEnumerable<ColourInvariant<T>> ColourInvariants { get; }
}
