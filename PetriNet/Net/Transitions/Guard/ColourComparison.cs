using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Transitions.Guard;

public class ColourComparison
{
    
    internal ColourComparison(ColourComparisonOperator @operator, IColourValue lhs)
    {
        Operator = @operator;
        Lhs = lhs;
    }
    internal ColourComparisonOperator Operator { get; set; }
    public IColourValue Lhs { get; protected set; }
    public override string ToString()
    {
        
        
        return base.ToString();
    }
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