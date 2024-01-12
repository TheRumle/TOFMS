using TACPN.Colours.Values;

namespace TACPN.Colours.Type;

public class SingletonColourType : ColourType
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
}