using TACPN.Net;

namespace TapaalParser.TapaalGui.XmlWriters.Symbols;

public class ColorIntervalDeclaration
{
    private readonly Interval _interval;
    private readonly string _colorType;
    private readonly string _color;

    public ColorIntervalDeclaration(ColoredGuard guard, ColourType colorType, string color)
    {
        if (!colorType.Colours.Contains(guard.Color)) throw new ArgumentException(guard + "had color " + guard.Color + " and colour type was " + colorType.Colours.Aggregate((f,e) => f + "," + e));
        this._interval = guard.Interval;
        this._colorType = colorType.Name;
        this._color = color;
    }

    public string ToXmlString()
    {
        return
            $@" <colorinterval> <inscription inscription=""[{_interval.Min},{_interval.Max}]""/> <colortype name=""{this._colorType}""> <color value=""{_color}""/> </colortype> </colorinterval>";
    }
}