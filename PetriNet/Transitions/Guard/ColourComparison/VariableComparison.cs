using TACPN.Colours.Values;
using TACPN.Exceptions;

namespace TACPN.Transitions.Guard.ColourComparison;

public sealed class VariableComparison : IColourComparison<ColourVariable, int>
{
    public VariableComparison(ColourComparisonOperator @operator, ColourVariable lhs, int rhs)
    {
        if (!lhs.VariableValues.AsInts.Contains(rhs))
            throw new InvalidColourVariableComparison(lhs, rhs);
        Operator = @operator;
        Lhs = lhs;
        Rhs = rhs;
    }

    public ColourComparisonOperator Operator { get; private set; }
    public ColourVariable Lhs { get; init; }
    public int Rhs { get; private set; }

    public static VariableComparison Equality(ColourVariable lhs, int rhs)
    {
        return new VariableComparison(ColourComparisonOperator.Eq, lhs,rhs);
    }
    
    
}