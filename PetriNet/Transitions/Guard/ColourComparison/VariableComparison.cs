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

    public override string ToString()
    {
        return Operator switch
        {
            ColourComparisonOperator.Eq => WithOperatorString("EQ"),
            ColourComparisonOperator.Gr => WithOperatorString("GR"),
            ColourComparisonOperator.Geq => WithOperatorString("GEQ"),
            ColourComparisonOperator.Le => WithOperatorString("LE"),
            ColourComparisonOperator.Leq => WithOperatorString("LEQ"),
            ColourComparisonOperator.Neq => WithOperatorString("NEQ"),
            _ => throw new ArgumentException($"Unexpected {nameof(ColourComparisonOperator)}")
        };
    }

    private string WithOperatorString(string comparison)
    {
        return $"{Rhs} {comparison.ToLower()} {Lhs.Name}";
    }
}