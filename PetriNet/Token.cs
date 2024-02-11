using TACPN.Colours.Expression;
using TACPN.Colours.Values;

namespace TACPN;

public record struct Token(IColourValue Colour, int Age = 0)
{
    public Token(string value, int age) : this(new Colour(value) as IColourValue,age)
    {}
}
