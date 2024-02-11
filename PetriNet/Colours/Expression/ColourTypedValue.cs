using TACPN.Colours.Type;

namespace TACPN.Colours.Expression;

public record ColourTypedValue : IColourTypedValue
{
    public string Value { get; }

    public ColourTypedValue(ColourType ct, string part)
    {
        ColourType = ct;
        this.Value = part;
        this.Part = part;
    }

    public readonly string Part;

    public ColourType ColourType { get; }
}