using Common;
using TACPN;
using TACPN.Colours.Type;

namespace TapaalParser.TapaalGui.XmlWriters.Symbols;

public class ColorIntervalDeclaration
{
    private readonly Interval _interval;
    private readonly string _colorType;
    private readonly string _color;

    public ColorIntervalDeclaration(ColourTimeGuard timeGuard, ColourType colorType, string color)
    {
        if (colorType != timeGuard.ColourType)
            throw new ArgumentException(timeGuard + "had color " + timeGuard.ColourType + " and colour type was " + colorType.ColourNames.Aggregate((f,e) => f + "," + e));
        
        _interval = timeGuard.Interval;
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