using TACPN.Colours.Type;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class EmptyBeforeCapacitorInhibitorAdaption : Adapter
{
    public EmptyBeforeCapacitorInhibitorAdaption(ISet<Location> locations, IndexedJourneyCollection collection, 
        ColourType partColourType) : base(partColourType, collection)
    {
        _locations = locations;
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