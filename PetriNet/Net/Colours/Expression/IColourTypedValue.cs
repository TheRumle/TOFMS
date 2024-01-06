using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public interface IColourTypedValue : IColourValue
{
    public ColourType ColourType { get; }
}