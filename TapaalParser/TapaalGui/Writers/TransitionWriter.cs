using TACPN.Places;
using TACPN.Transitions;
using TACPN.Transitions.Guard;

namespace TapaalParser.TapaalGui.Writers;

public class TransitionWriter : TacpnUiXmlWriter<Transition>
{
    public TransitionWriter(Transition value) : base(value)
    {
    }

    public override void AppendAllText()
    {
        Append($@"<net active=""true"" id=""{Parseable.Name}"" type=""P/T net"">");
        foreach (var place in Parseable.InvolvedPlaces)
            WritePlaces(place);

        WriteTransition(Parseable);


    }

    private void WriteTransition(Transition transition)
    {
        Append($@"<transition angle=""0"" displayName=""true"" id=""{transition.Name}"" infiniteServer=""false"" name=""{transition.Name}"" nameOffsetX=""0"" nameOffsetY=""0"" player=""0"" positionX=""50"" positionY=""50"" priority=""0"" urgent=""false""> ");
        WriteGuard(transition.Guard);
        Append(@"</transition>");
    }

    private void WriteGuard(ITransitionGuard transitionGuard)
    {
        if(transitionGuard == TransitionGuard.Empty) return;
        
        Append(@$"<condition>");
        Append($@"<text>");
        Append(transitionGuard.ToTapaalText());
        Append($@"</text>");
        
        
        
        
        
        
        
        
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

        WriteInvariants(location);
        Append(@"</place>");
    }

    private void WriteInvariants(Place place)
    {
        InvariantWriter writer = new InvariantWriter(place.ColourInvariants);
        writer.AppendAllText();
        Append(writer.ToString());
    }
}