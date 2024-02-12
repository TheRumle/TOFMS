using TACPN.Places;
using TACPN.Transitions;
using TapaalParser.TapaalGui.Writers;

namespace TapaalParser.TapaalGui;

public class PetriNetComponentWriter : TacpnUiXmlWriter<IEnumerable<Transition>>
{
    public PetriNetComponentWriter(IEnumerable<Transition> netTransitions): base(netTransitions)
    {
        
    }

    public override void AppendAllText()
    {
        foreach (var transition in Parseable)
        {
            Append($@"<net active=""true"" id=""{transition.Name}"" type=""P/T net"">");
            foreach (var place in transition.InvolvedPlaces)
            {
                WritePlaceUsage(place);
                var invs = new InvariantWriter(place.ColourInvariants);
                invs.AppendAllText();
                Append(invs.ToString());
            }
            
            var placeWriter = new PlaceWriter(transition.InvolvedPlaces);
            placeWriter.AppendAllText();

            Append($@"<net active=""true"" id=""{transition.Name}"" type=""P/T net"">");
        }
    }

    private void WritePlaceUsage(Place place)
    {
        Append($@"<place displayName=""true"" id=""{place.Name}"" initialMarking=""0"" invariant=""&lt; inf"" name=""{place.Name}"" nameOffsetX=""30"" nameOffsetY=""30"" positionX=""30"" positionY=""30"">
          <type>
            <text>{place.ColourType.Name}</text>
            <structure>
              <usersort declaration=""{place.ColourType.Name}""/>
            </structure>
          </type>");
    }
}