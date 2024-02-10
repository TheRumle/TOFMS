using TACPN.Places;

namespace TapaalParser.TapaalGui.Writers;

public class LocationWriter : TacpnUiXmlWriter<IEnumerable<Place>>
{
    public LocationWriter(IEnumerable<Place> value) : base(value)
    {
    }

    public override void AppendAllText()
    {
        foreach (var place in Parseable)
        {
            
        }
    }
}