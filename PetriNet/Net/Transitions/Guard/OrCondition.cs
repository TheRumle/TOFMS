namespace TACPN.Net.Transitions.Guard;
internal record OrCondition<T>(ColourComparison<T> Comparison) : ICondition<T>
{
    ColourComparison ICondition.Comparison => Comparison;
    public BooleanOperator Operator { get; } = BooleanOperator.Or;
}

internal record OrCondition(ColourComparison Comparison) : ICondition
{
    public BooleanOperator Operator { get; } = BooleanOperator.Or;
}