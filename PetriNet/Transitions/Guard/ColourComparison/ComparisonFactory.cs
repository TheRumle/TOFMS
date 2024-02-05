using TACPN.Colours.Values;

namespace TACPN.Transitions.Guard.ColourComparison;

public static class ComparisonFactory
{
    public static IColourComparison<ColourVariable, int> VariableComparison(ColourVariable colourVariable,
        ColourComparisonOperator comparisonOperator, int value)
    {
        return new VariableComparison(comparisonOperator, colourVariable, value);

    }
    
    
    public static VariableComparison Equality(ColourVariable lhs, int rhs)
    {
        return new VariableComparison(ColourComparisonOperator.Eq, lhs,rhs);
    }
    
    public static VariableComparison LessThan(ColourVariable lhs, int rhs)
    {
        return new VariableComparison(ColourComparisonOperator.Le, lhs,rhs);
    }
    
    public static VariableComparison LessThanEqual(ColourVariable lhs, int rhs)
    {
        return new VariableComparison(ColourComparisonOperator.Leq, lhs,rhs);
    }
    
    public static VariableComparison GreaterThanEqual(ColourVariable lhs, int rhs)
    {
        return new VariableComparison(ColourComparisonOperator.Geq, lhs,rhs);
    }
    
    public static VariableComparison GreaterThan(ColourVariable lhs, int rhs)
    {
        return new VariableComparison(ColourComparisonOperator.Gr, lhs,rhs);
    }
    
    public static VariableComparison NotEqual(ColourVariable lhs, int rhs)
    {
        return new VariableComparison(ColourComparisonOperator.Neq, lhs,rhs);
    }

}