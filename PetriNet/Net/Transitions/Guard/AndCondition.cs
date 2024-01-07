namespace TACPN.Net.Transitions.Guard;

internal record AndCondition(ColourComparison Comparison) : ICondition
{
    public BooleanOperator Operator { get; } = BooleanOperator.And;
}

internal record AndCondition<T>(ColourComparison<T> Comparison) : ICondition<T>
{
    ColourComparison ICondition.Comparison => Comparison;
    public BooleanOperator Operator { get; } = BooleanOperator.And;
}