using TACPN.Places;
using TACPN.Transitions;

namespace TapaalParser.TapaalGui.Writers;

public class TransitionWriter : TacpnUiXmlWriter<Transition>
{
    public TransitionWriter(Transition value) : base(value)
    {
    }

    public override void AppendAllText()
    {
        Append($@"<net active=""true"" id=""{Parseable.Name}"" type=""P/T net"">");
        var places = Parseable.InvolvedPlaces;
        foreach (var location in places)
            WritePlaces(location);
        
        
    }

    private void WritePlaces(Place location)
    {
        Append(
            $@"<place displayName=""true"" id=""{location.Name}"" initialMarking=""0"" invariant=""&lt; inf"" name=""{location.Name}"" nameOffsetX=""30"" nameOffsetY=""30"" positionX=""30"" positionY=""30"">
                      <type>
                        <text>{location.ColourType.Name}</text>
                        <structure>
                          <usersort declaration=""{location.ColourType.Name}""/>
                        </structure>
                      </type>");

        WritePlace(location);
        Append($@"</place>");
    }

    private void WritePlace(Place place)
    {
        InvariantWriter writer = new InvariantWriter(place.ColourInvariants);
        writer.AppendAllText();
        Append(writer.ToString());

    }
}