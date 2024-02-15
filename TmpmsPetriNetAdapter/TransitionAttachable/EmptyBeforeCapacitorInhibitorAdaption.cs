using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms;
using Tmpms.Journey;
using Tmpms.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class EmptyBeforeCapacitorInhibitorAdaption : Adapter
{

    public EmptyBeforeCapacitorInhibitorAdaption(MoveAction moveAction, ColourTypeFactory ctFactory, JourneyCollection collection) 
        : base(ctFactory)
    {
        _locations = moveAction.EmptyBefore;
    }

    private readonly IEnumerable<Location> _locations;

    public override void AttachToTransition(Transition transition)
    {
        foreach (var location in _locations)
        {
            var matchingPlace = transition.InvolvedPlaces.FirstOrDefault(place => place.Name == location.Name);
            if (matchingPlace != null)
                transition.AddInhibitorFrom(matchingPlace, 1);
            else
            {
                transition.AddInhibitorFrom(PlaceFactory.CreatePlace(location), 1);
            }
        }
    }
}