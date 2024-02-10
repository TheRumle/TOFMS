using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

public record ProductColourType : ColourType
{
    private ProductColourType(string name, IEnumerable<Colour> colours) : base(name, colours)
    {
    }

    public ProductColourType(ColourType first, ColourType second):this(first.Name+second.Name, [..first.Colours, ..second.Colours])
    {
        this.First = first;
        this.Second = second;
    }

    
    public ProductColourType(string name, ColourType first, ColourType second):this(name, [..first.Colours, ..second.Colours])
    {
        this.First = first;
        this.Second = second;
    }

    public ColourType Second { get; set; }

    public ColourType First { get; set; }
}