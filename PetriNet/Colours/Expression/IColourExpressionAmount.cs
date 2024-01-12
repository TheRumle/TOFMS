namespace TACPN.Colours.Expression;

public interface IColourExpressionAmount : IColourExpression
{
    public int Amount { get; internal set; }
}