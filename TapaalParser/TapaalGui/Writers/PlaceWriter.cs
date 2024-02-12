using Common;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;

namespace TapaalParser.TapaalGui.Writers;

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
            if (place.ColourInvariants.Count() == 1 && place.ColourInvariants.First().MaxAge == Infteger.PositiveInfinity)
            {
                Append($@"<shared-place initialMarking=""{place.Marking.Size}"" invariant=""&lt; inf"" name=""{place.Name}"">");
                WritePlaceColors(place);
                Append("</shared-place>");
                continue;
            }

            Append($@"<shared-place initialMarking=""{place.Marking.Size}"" invariant=""&lt; inf"" name=""{place.Name}"">");
            if (place.ColourInvariants.Count()>1 || place.ColourInvariants.First().ColourType != ColourType.DefaultColorType)
                WriteAllInvariants(place);
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



    private void WriteAllInvariants(Place place)
    {
        InvariantWriter writer = new InvariantWriter(place.ColourInvariants);
        writer.AppendAllText();
        Append(writer.ToString());
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


}