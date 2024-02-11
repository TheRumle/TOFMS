using Common;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;

namespace TACPN.Colours;

public record ColourInvariant(ColourType ColourType, IColourValue Colour, int MaxAge)
{
    public static ColourInvariant DotDefault = new ColourInvariant(ColourType.DefaultColorType,
        ColourType.DefaultColorType.ColourValue, InfinityInteger.Positive);
}

