using Common;
using TACPN.Colours;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;

namespace TapaalParser.TapaalGui.Writers;

public static class XmlUtils
{
    public static string GetInvariantTagText(ColourInvariant inv)
    {
        return inv.MaxAge == Infteger.PositiveInfinity ? "&lt; inf" : $"&lt;= {inv.MaxAge}";
    }
}

public class PlaceWriter : TacpnUiXmlWriter<IEnumerable<Place>>
{
    public PlaceWriter(IEnumerable<Place> value) : base(value)
    {
    }

    public override void AppendAllText()
    {
        foreach (var place in Parseable)
        {
            if (place.ColourInvariants.Count() == 1 &&
                place.ColourInvariants.First().ColourType == ColourType.DefaultColorType)
            {
                WriteNonColouredPlace(place);
                continue;
            }
            
            Append($@"<shared-place initialMarking=""{place.Marking.Size}"" invariant=""&lt; inf"" name=""{place.Name}"">");
            
            if (place.ColourInvariants.Any(inv => inv.MaxAge != Infteger.PositiveInfinity))
                WriteInvariants(place);
            
            
            WritePlaceColors(place);
            Append("</shared-place>");
        }
    }

    private void WritePlaceColors(Place place)
    {
        Append(@$"<type>");
        Append($@"<text>{place.ColourType.Name}</text>
                      <structure>
                        <usersort declaration=""{place.ColourType.Name}""/>
                      </structure>");
        Append(@$"</type>");
        
    }

    private void WriteInvariants(Place place)
    {
        WriteAllInvariants(place);
    }

    private void WriteAllInvariants(Place place)
    {
        foreach (var invariant in place.ColourInvariants)//TODO No colour invariant is assigned so this does not work. It migth be a test setup thing.
        {
            Append($@"<colorinvariant> <inscription inscription=""{XmlUtils.GetInvariantTagText(invariant)}""/><colortype name=""{invariant.ColourType.Name}"">");
            
            if (invariant.Colour is TupleColour t)
                WriteTupleInvariant(t);
            else
                WriteColourValue(invariant.Colour);

            Append($@"</colortype> </colorinvariant>");
        }
    }

    private void WriteNonColouredPlace(Place place)
    {
        var ltText = place.ColourInvariants.First().MaxAge == Infteger.PositiveInfinity
            ? "&lt; inf"
            : $"&lte; {place.ColourInvariants.First().MaxAge}";


        var numTokens = place.Marking.Size;
        var dot = Colour.DefaultTokenColour.Value;
        Append($@"<shared-place initialMarking=""{numTokens}"" invariant=""{ltText}"" name=""{place.Name}"">
                                <type>
                                  <text>{place.ColourType.Name}</text>
                                  <structure>
                                    <usersort declaration=""{place.ColourType.Name}""/>
                                  </structure>
                                </type>
                                <hlinitialMarking>
                                  <text>({numTokens}'{dot})</text>
                                  <structure>
                                    <add>
                                      <subterm>
                                        <numberof>
                                          <subterm>
                                            <numberconstant value=""{place.Marking.Size}"">
                                              <positive/>
                                            </numberconstant>
                                          </subterm>
                                          <subterm>
                                            <useroperator declaration=""{place.ColourType.Name}""/>
                                          </subterm>
                                        </numberof>
                                      </subterm>
                                    </add>
                                  </structure>
                                </hlinitialMarking>
                              </shared-place>");
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