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
        if (colorType.Name == ColourType.DefaultColorType.Name)
            this._colorType = ColourType.DefaultColorType.Name.ToLower();
        _colorType = colorType.Name;
        _color = color;
    }

    public string ToXmlString()
    {
        var maxString = _interval.Max == InfinityInteger.Positive ? "inf" : _interval.Max.ToString();
        return
            $@" <colorinterval> <inscription inscription=""[{_interval.Min},{maxString}]""/> <colortype name=""{_colorType}""> <color value=""{_color}""/> </colortype> </colorinterval>";
    }
}