namespace TACPN.Net.Transitions.Guard;
public interface ICondition
{
    ColourComparison Comparison { get; }
    BooleanOperator Operator { get; }
}

public interface ICondition<T> : ICondition
{
    new ColourComparison<T> Comparison { get; }
}