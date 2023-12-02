namespace TACPN.Net;
public record struct Token(string Colour,  int Age = 0);

public class TokenCollection : List<Token>
{
    public required IEnumerable<string> Colours { get; init; }

    public TokenCollection(IEnumerable<Token>tokens): base(tokens)
    {
    }
    public TokenCollection()
    {
    }

    public bool TryAdd(Token item)
    {
        if (!Colours.Contains(item.Colour)) return false;
        base.Add(item);
        return true;
    }
    
    public new void Add(Token item)
    {
        if (!TryAdd(item)) 
            throw new ArgumentException($"Token colour is {item.Colour} but the collection is of colours [{Colours.Aggregate((prev, curr)=>$@"{prev}, + {curr}")}]" );
    }
}