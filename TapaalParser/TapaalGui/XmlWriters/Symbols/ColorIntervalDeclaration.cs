using TACPN.Net;
using Tofms.Common;

namespace TapaalParser.TapaalGui.XmlWriters.Symbols;

public class ColorIntervalDeclaration
{
    private readonly Interval _interval;
    private readonly string _colorType;
    private readonly string _color;

    public ColorIntervalDeclaration(ColoredGuard guard, ColourType colorType, string color)
    {
        if (!colorType.Colours.Contains(guard.Color)) throw new ArgumentException(guard + "had color " + guard.Color + " and colour type was " + colorType.Colours.Aggregate((f,e) => f + "," + e));
        _interval = guard.Interval;
        _colorType = colorType.Name;
        if (colorType.Name == ColourType.DefaultColorType.Name)
            this._colorType = ColourType.DefaultColorType.Name.ToLower();
        
        _color = color;
    }

    public string ToXmlString()
    {
        var isInf = _interval.Max == InfinityInteger.Positive;
        var maxString = isInf ? "inf" : _interval.Max.ToString();
        var intervalEndSymbol = isInf ? ')' : ']'; 
        return 
            $@" <colorinterval> <inscription inscription=""[{_interval.Min},{maxString}{intervalEndSymbol}""/> <colortype name=""{_colorType}""> <color value=""{_color}""/> </colortype> </colorinterval>";
    }
}