using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Transitions;


public enum BooleanOperator
{
    Or,
    And
}

public interface IBooleanCombination<T>
{
    ColourComparison Comparison { get; }

    BooleanOperator? Operator { get; }
    ColourComparison? NestedComparison { get; }
}

public class ColourComparison
{
    public ColourComparison(ColourComparisonOperator @operator, IColourValue lhs)
    {
        Operator = @operator;
        Lhs = lhs;
    }
    public ColourComparisonOperator Operator { get; protected set; }
    public IColourValue Lhs { get; protected set; }
}
public class ColourComparison<T> : ColourComparison
{
    public ColourComparison(IColourValue lhs, ColourComparisonOperator colourComparisonOperator, T rhs): base( colourComparisonOperator, lhs)
    {
        if (rhs is not IColourValue or int) throw new ArgumentException("The rhs of a colour comparison must be an integer or colour value");
        this.Rhs = rhs;
    }
    
    public T Rhs { get; private set; }

}