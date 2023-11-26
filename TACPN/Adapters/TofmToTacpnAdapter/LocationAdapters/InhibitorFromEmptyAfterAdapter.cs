using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.LocationAdapters;

internal class InhibitorFromEmptyAfterAdapter : ITransitionAttachable
{
    private readonly IEnumerable<Location> _emptyAfterLocations;

    public InhibitorFromEmptyAfterAdapter(IEnumerable<Location> emptyAfterLocations)
    {
        this._emptyAfterLocations = emptyAfterLocations;
    }
    public void AttachToTransition(Transition transition)
    {
        foreach (var place in _emptyAfterLocations.Select(LocationTranslator.CreateLocation))
            transition.AddInhibitorFrom(place, 1);
    }
}