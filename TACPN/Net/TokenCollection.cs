using TACPN.Net.Colours.Type;

namespace TACPN.Net;

public class TokenCollection : List<Token>
{
    public ColourType ColourType { get; set; }
    public IEnumerable<string> Colours { get; set; }

    public static TokenCollection DotColorTokenCollection(int amount)
    {
        var tokens = Enumerable.Repeat(() => { return new Token("dot", 0); }, amount);
        return new TokenCollection()
        {
            ColourType = ColourType.DefaultColorType,
            Colours = tokens.Select(e=>e.Invoke()).Select(e=>e.Colour)
        };

    }

    public new void Add(Token item)
    {
        if (!Colours.Contains(item.Colour))
            throw new ArgumentException($"Token colour is {item.Colour} but the collection is of colours [{Colours.Aggregate((prev, curr)=>$@"{prev}, + {curr}")}]" );
        base.Add(item);
    }


    public int AmountOfColour(string colour)
    {
        if (!Colours.Contains(colour)) return 0;
        return FindAll(e => e.Colour == colour).Count;
    }

    public string ToColourExpression()
    {
        var numCol = new Dictionary<string, int>();
        foreach (var a in this)
        {
            if (numCol.ContainsKey(a.Colour))
                numCol[a.Colour] += 1;
            else
                numCol[a.Colour] = 1;

        }

        string value = "";
        foreach (var kvp in numCol)
        {
            value += $"({kvp.Value})'{kvp.Key}";
        }

        return value;


    }
}