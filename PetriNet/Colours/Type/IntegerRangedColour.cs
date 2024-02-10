using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

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