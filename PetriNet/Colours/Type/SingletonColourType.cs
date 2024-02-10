using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

public record SingletonColourType : ColourType
{
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