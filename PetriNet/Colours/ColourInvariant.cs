using Common;
using TACPN.Colours.Type;

namespace TACPN.Colours;

public record ColourInvariant(int MaxAge)
{
    public static ColourInvariant<string> DotDefault = new ColourInvariant<string>(ColourType.DefaultColorType,
        ColourType.DefaultColorType.ColourValue, InfinityInteger.Positive);
}

public record ColourInvariant<T1>(ColourType ColourType, T1 FirstColour, int MaxAge ) : ColourInvariant(MaxAge);
