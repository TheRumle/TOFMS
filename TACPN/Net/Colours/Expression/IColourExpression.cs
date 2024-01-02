using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public interface IColourExpression
{
    public ColourType ColourType { get; }
    public string Expression { get; }
}