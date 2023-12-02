namespace TACPN.Net;
public record Token(string Colour,  int Age = 0);

public class TokenCollection : List<Token>
{
    public required List<string> Colours { get; init; }

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

    public static TokenCollection Singleton(string colour, int amount)
    {
        var elements = Enumerable.Repeat(new Token(colour, 0), amount);
        var collection = new TokenCollection()
        {
            Colours = Enumerable.Repeat(colour,1).ToList()
        };
        collection.AddRange(elements);
        return collection;
    }

    public int AmountOfColour(string colour)
    {
        if (!Colours.Contains(colour)) return 0;
        return FindAll(e => e.Colour == colour).Count;
    }
}