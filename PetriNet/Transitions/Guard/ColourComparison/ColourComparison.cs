using TACPN.Colours.Expression;

namespace TACPN.Transitions.Guard.ColourComparison;

public interface IColourComparison<out TFirst, out TSecond> where TFirst : IColourValue
{
    ColourComparisonOperator Operator { get;}
    TFirst Lhs { get; }
    TSecond Rhs { get; }
}