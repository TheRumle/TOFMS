using TACPN.Colours.Type;

namespace TACPN.Colours.Expression;

public record PartColourValue(string Part) : IColourTypedValue
{
    public string Value { get; } = Part;
    public ColourType ColourType { get; } = ColourType.PartsColourType;
}