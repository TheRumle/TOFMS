using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public interface IColourTypedColourValue : IColourValue
{
    public ColourType ColourType { get; }
}