using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

public record SingletonColourType : ColourType
{
    public enum NamingConvention
    {
        ExactSameAsValue,
        CapitalizeFirstLetter
    }
    public Colour ColourValue { get; }
    public SingletonColourType(string name, Colour color) : base(name, new []{color})
    {
        this.ColourValue = color;
    }

    public override string ToString()
    {
        return ColourValue.Value;
    }
}

public record IntegerRangedColour : ColourType
{
    public IntegerRangedColour(string name, int maxValue) : base(name, Enumerable.Range(0,maxValue).Select(e=>e.ToString()))
    {
        this.MaxValue = maxValue;
    }

    public int MaxValue { get; }

    public IntegerRangedColour(string name, IEnumerable<Colour> colours) : this(name, colours.Count())
    {
    }
}