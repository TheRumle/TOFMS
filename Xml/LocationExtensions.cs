using TACPN.Net;
using Tofms.Common;

namespace Xml;

public static class LocationExtensions
{
    public static CapacityLocation ToCapacityLocation(this Location location)
    {
        return new CapacityLocation(location.Name+Colours.Hat, location.Capacity);
    }
    
    public static Invariant Infinity = new Invariant(ColourType.DefaultColorType.Name, 0, Infteger.PositiveInfinity);
}