using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN.Adapters.Location;

internal class EmptyAfterInhibitorOnAllExceptFromCap : ITransitionAttachable
{
    public void AttachToTransition(Transition transition)
    {
        
    }
}

internal class EmptyBeforeCapacitorInhibitorAdaption : ITransitionAttachable
{
    public EmptyBeforeCapacitorInhibitorAdaption(ISet<Common.Location> locations)
    {
        Locations = locations;
    }

    public IEnumerable<Common.Location> Locations { get; }

    public void AttachToTransition(Transition transition)
    {
        foreach (var place in Locations.Select(LocationTranslator.CreateLocation))
            transition.AddInhibitorFrom(place, CapacityPlaceCreator.DefaultColorName, 1);
    }
}