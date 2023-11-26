using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.LocationAdapters;

internal class EmptyBeforeCapacitorInhibitorAdaption : ITransitionAttachable
{
    public EmptyBeforeCapacitorInhibitorAdaption(ISet<Location> locations)
    {
        _locations = locations;
    }

    private readonly IEnumerable<Location> _locations;

    public void AttachToTransition(Transition transition)
    {
        foreach (var place in _locations.Select(LocationTranslator.CreateLocation))
            transition.AddInhibitorFrom(place, 1);
    }
}