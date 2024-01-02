using TACPN.Net.Colours.Type;
using Tofms.Common;

namespace TACPN.Net.Colours;

public record ColourInvariant(int MaxAge)
{
    public static ColourInvariant<string> DotDefault = new(ColourType.DefaultColorType, ColourType.DefaultColorType.Name, InfinityInteger.Positive);
}

public record ColourInvariant<T1>(ColourType ColourType, T1 FirstColour, int MaxAge ) : ColourInvariant(MaxAge);

public record ColourInvariant<T1,T2>(ColourType ColourType, T1 FirstColour, T2 SecondColour, int MaxAge) : ColourInvariant<T1>(ColourType, FirstColour, MaxAge);