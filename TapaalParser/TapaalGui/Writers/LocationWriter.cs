using Common;
using TACPN.Colours;
using TACPN.Colours.Expression;
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
            Append($@"<shared-place initialMarking=""0"" invariant=""&lt; inf"" name=""{place.Name}"">");
            WriteInvariants(place);
            Append($@"<type> <text>{place.ColourType.Name}</text> <structure><usersort declaration=""{place.ColourType.Name}""/> </structure> </type> </shared-place>");
        }
    }

    private void WriteInvariants(Place place)
    {
        
        foreach (var inv in place.ColourInvariants)
        {
            var invMaxText = inv.MaxAge == Infteger.PositiveInfinity ? "&lt; inf" : $"&lt;= {inv.MaxAge}";
            Append(
                $@"<colorinvariant> <inscription inscription=""{invMaxText}""/><colortype name=""{inv.ColourType.Name}"">");
            switch (inv.Colour)
            {
                case TupleColour t:
                    WriteTupleInvariant(t);
                    break;
                default:
                    WriteColourValue(inv.Colour);
                    break;
            }
            
            Append($@"</colortype> </colorinvariant>"")");
        }
        
    }

    private void WriteColourValue(IColourValue invColour)
    {
        Append($@"<color value=""{invColour.Value}""/>");
    }

    private void WriteTupleInvariant(TupleColour tupleColour)
    {
        var text = tupleColour.ColourComponents.Select(e => $@"<color value=""{e.Value}""/>")
            .Aggregate("",(s,e)=>s+e);

        Append(text);
    }
}