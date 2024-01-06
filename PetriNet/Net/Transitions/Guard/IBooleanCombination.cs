namespace TACPN.Net.Transitions.Guard;

public interface IBooleanCombination<T>
{
    ColourComparison Comparison { get; }

    BooleanOperator? Operator { get; }
    ColourComparison? NestedComparison { get; }
}