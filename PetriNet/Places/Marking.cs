using System.Runtime.CompilerServices;
using System.Text;
using TACPN.Colours.Expression;

namespace TACPN.Places;

public class Marking() : Dictionary<IColourValue, List<Token>>(new List<KeyValuePair<IColourValue, List<Token>>>())
{
    public int Size => Values.SelectMany(e => e).Count();
    public IEnumerable<Token> Tokens => this.Values.SelectMany(e=>e); 
    public override string ToString()
    {
        StringBuilder b = new StringBuilder();
        foreach (var (color, tokens) in this)
        {
            b.Append($"{color}: [");
            b.Append(string.Join(", ", tokens));
            b.Append(']');
        }

        return b.ToString();
    }

    public void AddToken(Token token)
    {
        if (TryGetValue(token.Colour, out var tokens))
        {
            tokens.Add(token);
            return;
        }

        this[token.Colour] = [token];
    }
    
    public void AddToken(IColourValue value, int age)
    {
        AddToken(new Token(value,age));
    }
    public void AddToken((IColourValue value, int age) token)
    {
        AddToken(new Token(token.value,token.age));
    }
}