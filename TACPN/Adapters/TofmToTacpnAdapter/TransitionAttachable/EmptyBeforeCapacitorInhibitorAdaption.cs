using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;

internal class EmptyBeforeCapacitorInhibitorAdaption : ITransitionAttachable
{
    public EmptyBeforeCapacitorInhibitorAdaption(ISet<Location> locations)
    {
        _locations = locations;
    }

    private readonly IEnumerable<Location> _locations;

    public void AttachToTransition(Transition transition)
    {
        foreach (var place in _locations.Select(LocationTranslator.CreatePlace))
            transition.AddInhibitorFrom(place, 1);
    }
}