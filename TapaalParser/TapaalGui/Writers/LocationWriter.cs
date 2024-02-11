using TACPN.Colours;
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
            //Append($@"<shared-place initialMarking=""0"" invariant=""&lt; inf"" name=""{place.Name}"">");
            //WriteInvariants(place);
            //Append($@"<type> <text>{Colours.TokenColour}</text> <structure><usersort declaration=""{Colours.TokenColour}""/> </structure> </type> </shared-place>");
        }
    }

    private void WriteInvariants(Place place)
    {
        /*
        foreach (var inv in place.ColourInvariants)
        {
            var part = inv.Colour;
            var v = collection[inv.PartType];
            foreach (var jourIndex in v)
            {
                int jourNumber = jourIndex.Key;

                WriteInvariant(inv, part, jourNumber);
            }
        }
        */
    }
}