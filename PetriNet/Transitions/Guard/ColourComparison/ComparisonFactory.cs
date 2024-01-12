using TACPN.Colours.Values;

namespace TACPN.Transitions.Guard.ColourComparison;

public static class ComparisonFactory
{
    public static IColourComparison<ColourVariable, int> VariableComparison(ColourVariable colourVariable,
        ColourComparisonOperator comparisonOperator, int value)
    {
        return new VariableComparison(comparisonOperator, colourVariable, value);

    }

}