using TACPN.Colours.Type;

namespace TACPN.Colours.Expression;

public interface IColourTypedValue : IColourValue
{
    public ColourType ColourType { get; }
}