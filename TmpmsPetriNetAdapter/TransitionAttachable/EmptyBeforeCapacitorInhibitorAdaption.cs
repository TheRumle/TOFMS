using TACPN.Transitions;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class EmptyBeforeCapacitorInhibitorAdaption : ITransitionAttachable
{
    public EmptyBeforeCapacitorInhibitorAdaption(ISet<Location> locations, IndexedJourney collection)
    {
        _locations = locations;
        _collection = collection;
    }

    private readonly IEnumerable<Location> _locations;
    private readonly IndexedJourney _collection;

    public void AttachToTransition(Transition transition)
    {
        foreach (var location in _locations)
        {
            var matchingPlace = transition.InvolvedPlaces.FirstOrDefault(place => place.Name == location.Name);
            if (matchingPlace != null)
                transition.AddInhibitorFrom(matchingPlace, 1);
            else
            {
                transition.AddInhibitorFrom(LocationTranslator.CreatePlace(location, _collection), 1);
            }
        }
    }
}