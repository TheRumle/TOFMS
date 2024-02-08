using TACPN.Colours.Type;

namespace TACPN.Colours.Expression;

public record PartColourValue : IColourTypedValue
{
    public string Value { get; }

    public PartColourValue(ColourType ct, string part)
    {
        ColourType = ct;
        this.Value = part;
        this.Part = part;
    }

    public readonly string Part;

    public ColourType ColourType { get; }
}