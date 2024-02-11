﻿using Common;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
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
            if (place.ColourInvariants.Count() == 1 &&
                place.ColourInvariants.First().ColourType == ColourType.DefaultColorType)
            {
                WriteNonColouredPlace(place);
                continue;
            }
            
            Append($@"<shared-place initialMarking=""{place.Marking.Size}"" invariant=""&lt; inf"" name=""{place.Name}"">");
            WriteInvariants(place);
            Append($@"<type> <text>{place.ColourType.Name}</text> <structure><usersort declaration=""{place.ColourType.Name}""/> </structure> </type> </shared-place>");
        }
    }

    private void WriteInvariants(Place place)
    {
        foreach (var inv in place.ColourInvariants)
        {
            var invMaxText = inv.MaxAge == Infteger.PositiveInfinity ? "&lt; inf" : $"&lt;= {inv.MaxAge}";
            Append($@"<colorinvariant> <inscription inscription=""{invMaxText}""/><colortype name=""{inv.ColourType.Name}"">");
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