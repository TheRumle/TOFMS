using TACPN.Colours.Values;
using TACPN.Exceptions;

namespace TACPN.Transitions.Guard.ColourComparison;

internal sealed class VariableComparison : IColourComparison<ColourVariable, int>
{
    internal VariableComparison(ColourComparisonOperator @operator, ColourVariable lhs, int rhs)
    {
        if (!lhs.VariableValues.AsInts.Contains(rhs))
            throw new InvalidColourVariableComparison(lhs, rhs);
        Operator = @operator;
        Lhs = lhs;
        Rhs = rhs;
    }

    public ColourComparisonOperator Operator { get; private set; }
    public ColourVariable Lhs { get; private set; }
    public int Rhs { get; private set; }
    
    
}