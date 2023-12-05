namespace TACPN.Net;
public record Token(string Colour,  int Age = 0);

public class TokenCollection : List<Token>
{
    public ColourType ColourType { get; set; }
    public IEnumerable<string> Colours { get; set; } 
    
    


    
    
    public TokenCollection(ColourType colourType, IEnumerable<Token>tokens) : base(tokens)
    {
        this.ColourType = colourType;
        this.Colours = colourType.Colours;
    }
    
    public TokenCollection(IEnumerable<Token>tokens) : base(tokens)
    {
        this.ColourType = new ColourType(tokens.First().Colour, new []{tokens.First().Colour});
        this.Colours = tokens.Select(e=>e.Colour);
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
        var collection = new TokenCollection(new ColourType(colour, new List<string>(){colour}), elements)
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