namespace TACPN.Net;
public record Token(string Colour,  int Age = 0);

public class TokenCollection : List<Token>
{
    public required IEnumerable<string> Colours { get; init; }

    public TokenCollection(IEnumerable<Token>tokens): base(tokens)
    {
    }
    public TokenCollection()
    {
    }

    public new void Add(Token item)
    {
        if (!Colours.Contains(item.Colour))
            throw new ArgumentException($"Token colour is {item.Colour} but the collection is of colours [{Colours.Aggregate((prev, curr)=>$@"{prev}, + {curr}")}]" );
        base.Add(item);
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