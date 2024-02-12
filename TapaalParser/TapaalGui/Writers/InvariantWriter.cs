using TACPN.Colours;
using TACPN.Colours.Expression;

namespace TapaalParser.TapaalGui.Writers;

internal class InvariantWriter : TacpnUiXmlWriter<IEnumerable<ColourInvariant>>
{
    public InvariantWriter(IEnumerable<ColourInvariant> value) : base(value)
    {
    }
    public override void AppendAllText()
    {
        foreach (var invariant in Parseable)
        {
            Append($@"<colorinvariant> <inscription inscription=""{XmlUtils.GetInvariantTagText(invariant)}""/><colortype name=""{invariant.ColourType.Name}"">");

            if (invariant.Colour is TupleColour t)
                WriteTupleInvariant(t);
            else
                WriteColourValue(invariant.Colour);

            Append($@"</colortype> </colorinvariant>");
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