using TACPN.Net.Colours.Values;

namespace TACPN.Net.Transitions.Guard;

internal class VariableComparison : ColourComparison<int>
{
    public VariableComparison(IColourVariableExpression lhs, ColourComparisonOperator colourComparisonOperator, int rhs)
        : base(lhs, colourComparisonOperator, rhs)
    {
    }

}