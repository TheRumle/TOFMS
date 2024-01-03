using TACPN.Net.Colours.Values;

namespace TACPN.Net.Transitions;

public class ColourComparison
{
    public ColourComparison(Colour lhs, BooleanOperator booleanOperator, Colour rhs)
    {
        this.Operator = booleanOperator;
        this.Rhs = rhs;
        this.Lhs = lhs;
    }

    public Colour Lhs { get; private set; }

    public Colour Rhs { get; private set; }

    public BooleanOperator Operator { get; private set; }
}