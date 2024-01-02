using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Places;

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

public interface IPlace<T, TT> : IPlace
{
    IEnumerable<ColourInvariant<T,TT>> ColourInvariants { get; }
}